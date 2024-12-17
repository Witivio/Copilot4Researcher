using System.Net.Http;
using System;
using System.Web;
using System.Xml.Serialization;
using System.Linq;
using Microsoft.Extensions.Logging.Console;
using Witivio.Copilot4Researcher.Features.Literature.Pubmed.Models;
using Witivio.Copilot4Researcher.Features.Literature.Models;
using Witivio.Copilot4Researcher.Core;

namespace Witivio.Copilot4Researcher.Features.Literature.Pubmed
{
    public interface IPubmedClient
    {
        Task<Publication> GetByIdAsync(string pubmedId);
        Task<IEnumerable<Publication>> SearchAsync(LiteratureSearchQuery query);
    }

    public class PubmedClient : IPubmedClient
    {
        private readonly HttpClient _httpClient;
        private readonly string[] _apiKeys;
        private int _currentKeyIndex = 0;
        private readonly object _lockObject = new object();

        public PubmedClient(HttpClient httpClient, IConfiguration configuration)
        {
            httpClient.BaseAddress = new Uri("https://eutils.ncbi.nlm.nih.gov/");
            _httpClient = httpClient;
            _apiKeys = configuration.GetSection("PubmedApiKeys").Get<string[]>() ??
                throw new ArgumentException("PubmedApiKeys configuration is required");
        }

        public async Task<IEnumerable<Publication>> SearchAsync(LiteratureSearchQuery query)
        {
            var articles = await GetArticlesUsingESearch(query);
            if (articles.ESearchResult.Count == "0")
                return new List<Publication>();

            var pubMedIds = articles.ESearchResult.Idlist;

            var articleDetails = await GetArticleDetailsUsingEfetchAsync(pubMedIds.ToArray());


            var citationTasks = pubMedIds.Select(id => GetCitationUsingELinkAsync(id));

            var citations = await Task.WhenAll(citationTasks);

            var publications = new List<Publication>();

            foreach (var article in articleDetails.PubmedArticles)
            {
                var citationCount = citations
                        .Where(c => c != null)
                        .Where(c => c.Linksets != null)
                        .SelectMany(c => c.Linksets)
                        .Where(linkset => linkset.Ids.Contains(article.MedlineCitation.Pmid))
                        .Where(linkset => linkset.Linksetdbs != null)
                        .SelectMany(linkset => linkset.Linksetdbs)
                        .Where(linksetDb => linksetDb.Links != null)
                        .SelectMany(linksetDb => linksetDb.Links)
                        .Count();



                var publication = new Publication
                {
                    Id = article.MedlineCitation?.Pmid ?? string.Empty,
                    Title = article.MedlineCitation?.Article?.GetArticleTitleTextAsPlainText() ?? string.Empty,
                    JournalName = article.MedlineCitation?.Article?.Journal?.Title ?? string.Empty,
                    Abstract = article.MedlineCitation?.Article?.Abstract?.GetAbstractTextAsPlainText(),
                    Authors = article.MedlineCitation?.Article?.AuthorList?.Authors is { Count: > 0 } authors
                        ? new Authors
                        {
                            First = FormatName(authors.First()),
                            Last = FormatName(authors.Last())
                        }
                        : null,
                    DOI = article.MedlineCitation?.Article?.ELocationId?.Text,
                    Citations = citationCount,
                    Link = $"https://pubmed.ncbi.nlm.nih.gov/{article.MedlineCitation?.Pmid}",
                    Source = Publication.PublicationSource.Pubmed
                };

                var date = article.PubmedData.History.PubMedPubDates.FirstOrDefault(p => p.PubStatus == "pubmed");
                if (date != null)
                {
                    publication.Date = new DateOnly(date.Year, date.Month, date.Day);
                }

                publications.Add(publication);
            }

            return publications;

        }

        private string GetNextApiKey()
        {
            lock (_lockObject)
            {
                var key = _apiKeys[_currentKeyIndex];
                _currentKeyIndex = (_currentKeyIndex + 1) % _apiKeys.Length;
                return key;
            }
        }

        private string FormatName(Author author)
        {
            return author.ForeName + " " + author.LastName;
        }

        private async Task<PubmedSearchResult> GetArticlesUsingESearch(LiteratureSearchQuery query)
        {
            string baseUrl = "/entrez/eutils/esearch.fcgi";
            var queryString = BuildSearchQuery(query).Replace("%2b", "+");
            string url = $"{baseUrl}?{queryString}&api_key={GetNextApiKey()}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadFromJsonAsync<PubmedSearchResult>();

            return res;
        }

        private async Task<PubmedArticleSet> GetArticleDetailsUsingEfetchAsync(params string[] pubMedIds)
        {
            string baseUrl = "/entrez/eutils/efetch.fcgi";
            string idList = string.Join(",", pubMedIds);
            string url = $"{baseUrl}?db=pubmed&id={idList}&retmode=xml&api_key={GetNextApiKey()}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                // Deserialize the XML response into the PubmedArticleSet object
                XmlSerializer serializer = new XmlSerializer(typeof(PubmedArticleSet));
                using (StringReader reader = new StringReader(responseBody))
                {
                    return (PubmedArticleSet)serializer.Deserialize(reader);
                }
            }
            catch (Exception)
            {


            }
            return null;

        }

        private async Task<PubmedLinkResult> GetCitationUsingELinkAsync(string pubMedId)
        {
            string baseUrl = "/entrez/eutils/elink.fcgi";

            string url = $"{baseUrl}?dbfrom=pubmed&linkname=pubmed_pmc_refs&id={pubMedId}&retmode=json&api_key={GetNextApiKey()}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            try
            {
                var str = await response.Content.ReadAsStringAsync();
                var res = await response.Content.ReadFromJsonAsync<PubmedLinkResult>();
                return res;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string BuildSearchQuery(LiteratureSearchQuery query)
        {
            var builder = HttpUtility.ParseQueryString(string.Empty);

            builder["db"] = "pubmed";
            builder["retmax"] = query.NbItems.ToString();
            builder["retmode"] = "json";
            builder["datetype"] = "pdat";
            builder["sort"] = "relevance";

            // Add keywords
            if (query.Keywords != null && query.Keywords.Any())
            {
                string keywords = string.Join("+OR+", query.Keywords.Select(k => $"{k}[All Fields]"));
                builder["term"] = keywords;
            }

            // Add authors
            if (query.Authors != null && query.Authors.Any())
            {
                string authors = string.Join("+AND+", query.Authors.Select(a => $"{a}[Author]"));
                builder["term"] += $"+AND+{authors}";
            }

            // Add date range
            if (query.MinDate.HasValue)
            {
                builder["mindate"] = query.MinDate.Value.ToString("yyyy/MM/dd");
            }


            if (query.MaxDate.HasValue)
            {
                builder["maxdate"] = query.MaxDate.Value.ToString("yyyy/MM/dd");
            }

            // Filter out preprints
            builder["term"] += "+NOT+preprint[pt]+hasabstract[text]";

            // Sort by date or relevance
            builder["sort"] = query.Sort == SortBy.Date ? "pubdate" : "relevance";

            if (query.Prioritze == Prioritze.Recent)
            {
                builder["mindate"] = DateTime.UtcNow.AddYears(-1).ToString("yyyy/MM/dd");
            }


            return builder.ToString();
        }

        public async Task<Publication> GetByIdAsync(string pubmedId)
        {
            var articleDetails = await GetArticleDetailsUsingEfetchAsync(pubmedId);
            var article = articleDetails.PubmedArticles.FirstOrDefault();
            if (article == null)
                return null;

            var citations = await GetCitationUsingELinkAsync(pubmedId);

            var citationCount = citations.Linksets
                         .Where(linkset => linkset.Ids.Contains(article.MedlineCitation.Pmid))
                         .Where(linkset => linkset.Linksetdbs != null)
                         .SelectMany(linkset => linkset.Linksetdbs)
                         .Where(linksetDb => linksetDb.Links != null)
                         .SelectMany(linksetDb => linksetDb.Links)
                         .Count();

            return new Publication
            {
                Id = article.MedlineCitation.Pmid,
                Title = article.MedlineCitation.Article.GetArticleTitleTextAsPlainText(),
                JournalName = article.MedlineCitation.Article.Journal.Title,
                Abstract = article.MedlineCitation.Article.Abstract?.GetAbstractTextAsPlainText(),
                Authors = new Authors
                {
                    First = FormatName(article.MedlineCitation.Article.AuthorList.Authors.First()),
                    Last = FormatName(article.MedlineCitation.Article.AuthorList.Authors.Last())
                },
                DOI = article.MedlineCitation.Article.ELocationId?.Text,
                Citations = citationCount,
                Link = "https://pubmed.ncbi.nlm.nih.gov/" + article.MedlineCitation.Pmid,
                Source = Publication.PublicationSource.Pubmed
            };

        }
    }
}
