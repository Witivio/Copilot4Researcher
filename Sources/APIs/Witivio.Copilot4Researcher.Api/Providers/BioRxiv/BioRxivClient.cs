using HtmlAgilityPack;
using System.Web;
using Witivio.Copilot4Researcher.Models;
using Witivio.Copilot4Researcher.Providers.BioRxiv.Models;

namespace Witivio.Copilot4Researcher.Providers.BioRxiv
{
    public interface IBioRxivClient
    {
        Task<IEnumerable<Publication>> SearchAsync(LiteratureSearchQuery query);
    }

    public class BioRxivClient : IBioRxivClient
    {
         private const string DOI_PREFIX = "https://doi.org/";
        private readonly HttpClient _httpClient;

        public BioRxivClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Publication>> SearchAsync(LiteratureSearchQuery query)
        {
            var articles = await GetArticles(query);

            return articles;

        }

        private async Task<IEnumerable<Publication>> GetArticles(LiteratureSearchQuery query)
        {
            string baseUrl = "https://www.biorxiv.org/search/";
            var queryString = BuildSearchQuery(query);
            string url = $"{baseUrl}{queryString}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var publications = doc.DocumentNode.Descendants("li").Where(node => node.HasClass("search-result"))
                .Select(c => new Publication
                {
                    Title = c.Descendants("span").FirstOrDefault(n => n.HasClass("highwire-cite-title"))?.InnerText.Trim(),
                    Link = "https://www.biorxiv.org/" + c.Descendants("a").FirstOrDefault(n => n.HasClass("highwire-cite-linked-title"))?.GetAttributeValue("href", ""),
                    JournalName = "bioRxiv",
                    Source = "bioRxiv",
                    Date = FormatToDateOnly(c.Descendants("span").FirstOrDefault(n => n.HasClass("highwire-cite-metadata-pages"))?.InnerText),
                    DOI = RemoveSchema(c.Descendants("span").FirstOrDefault(n => n.HasClass("highwire-cite-metadata-doi"))?.InnerText),
                    Authors = new Authors
                    {
                        First = c.Descendants("span").FirstOrDefault(n => n.HasClass("highwire-citation-author") && n.HasClass("first"))?.InnerText,
                        Last = c.Descendants("span").LastOrDefault(n => n.HasClass("highwire-citation-author"))?.InnerText,
                    }
                }).ToList();

            var tasks = publications.Select(p => GetArticleDetailAsync(p));

            var articleDetails = await Task.WhenAll(tasks);

            publications.ForEach(publication =>
            {
                publication.Abstract = articleDetails
                    .FirstOrDefault(a => string.Equals(a.Doi, publication.DOI, StringComparison.InvariantCultureIgnoreCase))
                    ?.Abstract;
            });

            return publications;
        }

        private string RemoveSchema(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            
            var doi = text.Substring(4).Trim(); // remove "doi:"
            return doi.StartsWith(DOI_PREFIX) ? doi.Substring(DOI_PREFIX.Length) : doi;
        }

        private async Task<BioRxivArticleDetail> GetArticleDetailAsync(Publication publication)
        {
            string baseUrl = "https://api.biorxiv.org/details/biorxiv";

            string url = $"{baseUrl}/{publication.DOI}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadFromJsonAsync<BioRxivArticlesResult>();

            var article = res.ArticleDetails.OrderBy(b => b.Version).Select(b => b).LastOrDefault();

            return article;
        }


        private DateOnly? FormatToDateOnly(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
            {
                throw new ArgumentException("Input date string cannot be null or empty.", nameof(dateString));
            }

            // Split the string by '.' delimiter
            var dateParts = dateString.Trim().Split('.');

            // Ensure there are at least 3 parts (Year, Month, Day)
            if (dateParts.Length < 3 || !int.TryParse(dateParts[0], out int year) ||
                !int.TryParse(dateParts[1], out int month) || !int.TryParse(dateParts[2], out int day))
            {
                return null; // Invalid format
            }

            return DateOnly.TryParse(dateString, out DateOnly result) ? result : null;
        }

        public string BuildSearchQuery(LiteratureSearchQuery query)
        {
            // Escape and format user input keywords
            var formattedKeywords = query.Keywords != null ? string.Join("%20", query.Keywords) : string.Empty;

            // Prepare author query if provided
            var authorQuery = query.Authors != null && query.Authors.Length > 0
                ? "author1%3A" + string.Join("%20author1%3A", query.Authors.Select(HttpUtility.UrlEncode))
                : string.Empty;

            // Start building the query parameters
            var queryParameters = new List<string>();

            if (!string.IsNullOrEmpty(formattedKeywords))
            {
                queryParameters.Add(formattedKeywords);
            }

            if (!string.IsNullOrEmpty(authorQuery))
            {
                queryParameters.Add(authorQuery);
            }

            // Add date filters if provided
            if (query.MinDate.HasValue)
            {
                queryParameters.Add($"filter%3Adate-from%3A{query.MinDate.Value:yyyy-MM-dd}");
            }

            if (query.MaxDate.HasValue)
            {
                queryParameters.Add($"filter%3Adate-to%3A{query.MaxDate.Value:yyyy-MM-dd}");
            }

            // Add sort, format, and other static parameters as in the example
            queryParameters.Add("jcode%3Abiorxiv");
            queryParameters.Add("numresults%3A" + query.NbItems);
            queryParameters.Add("sort%3Arelevance-rank");
            queryParameters.Add("format_result%3Astandard");

            // Join all parameters into the final URL
            string queryString = string.Join("%20", queryParameters);

            return queryString;
        }
    }
}
