using HtmlAgilityPack;
using System.Web;
using Witivio.Copilot4Researcher.Core;
using Witivio.Copilot4Researcher.Features.Literature.BioRxiv.Models;
using Witivio.Copilot4Researcher.Features.Literature.Models;

namespace Witivio.Copilot4Researcher.Features.Literature.BioRxiv
{
    public interface IBioRxivClient
    {
        /// <summary>
        /// Searches for publications based on the provided query parameters
        /// </summary>
        /// <param name="query">Search parameters including keywords, authors, dates, and sorting preferences</param>
        /// <returns>A collection of matching publications</returns>
        Task<IEnumerable<Publication>> SearchAsync(LiteratureSearchQuery query);
    }

    public class BioRxivClient : IBioRxivClient
    {
        private const string DOI_PREFIX = "https://doi.org/";
        private const string BASE_URL = "https://www.biorxiv.org";
        private const string API_URL = "https://api.biorxiv.org";
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

        /// <summary>
        /// Retrieves articles based on the provided search query
        /// </summary>
        /// <param name="query">Search parameters including keywords, authors, dates, and sorting preferences</param>
        /// <returns>A collection of publications matching the search criteria</returns>
        private async Task<IEnumerable<Publication>> GetArticles(LiteratureSearchQuery query)
        {
            // Build and validate the search URL
            string baseUrl = $"{BASE_URL}/search/";
            var queryString = BuildSearchQuery(query);
            string url = $"{baseUrl}{queryString}";

            try
            {
                // Fetch and parse HTML content
                var html = await FetchHtmlContentAsync(url);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                // Extract publications from search results
                var publications = doc.DocumentNode
                    .Descendants("li")
                    .Where(node => node.HasClass("search-result"))
                    .Select(node => ParsePublicationFromNode(node))
                    .Where(pub => pub != null)
                    .ToList();

                // Enrich publications with abstracts in parallel
                await EnrichPublicationsWithAbstracts(publications);

                return publications;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to fetch articles from BioRxiv", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing BioRxiv articles", ex);
            }
        }

        /// <summary>
        /// Fetches HTML content from the specified URL
        /// </summary>
        /// <param name="url">The URL to fetch content from</param>
        /// <returns>The HTML content as a string</returns>
        private async Task<string> FetchHtmlContentAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Parses an HTML node to extract publication information
        /// </summary>
        /// <param name="node">The HTML node containing publication data</param>
        /// <returns>A Publication object containing the extracted information</returns>
        private Publication ParsePublicationFromNode(HtmlNode node)
        {
            return new Publication
            {
                Title = ExtractNodeText(node, "span", "highwire-cite-title"),
                Link = $"{BASE_URL}{ExtractNodeAttribute(node, "a", "highwire-cite-linked-title", "href")}",
                JournalName = "bioRxiv",
                Source = Publication.PublicationSource.BioRxiv,
                Date = FormatToDateOnly(ExtractNodeText(node, "span", "highwire-cite-metadata-pages")),
                DOI = RemoveSchema(ExtractNodeText(node, "span", "highwire-cite-metadata-doi")),
                Authors = new Authors
                {
                    First = ExtractNodeText(node, "span", "highwire-citation-author first"),
                    Last = ExtractLastAuthorText(node)
                }
            };
        }

        /// <summary>
        /// Extracts text content from an HTML node based on element name and class
        /// </summary>
        /// <param name="node">The parent HTML node to search in</param>
        /// <param name="elementName">The name of the HTML element to find</param>
        /// <param name="className">The class name to match</param>
        /// <returns>The extracted text content or empty string if not found</returns>
        private string ExtractNodeText(HtmlNode node, string elementName, string className)
        {
            return node.Descendants(elementName)
                .FirstOrDefault(n => n.HasClass(className))
                ?.InnerText
                ?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Extracts an attribute value from an HTML node based on element name and class
        /// </summary>
        /// <param name="node">The parent HTML node to search in</param>
        /// <param name="elementName">The name of the HTML element to find</param>
        /// <param name="className">The class name to match</param>
        /// <param name="attributeName">The name of the attribute to extract</param>
        /// <returns>The attribute value or empty string if not found</returns>
        private string ExtractNodeAttribute(HtmlNode node, string elementName, string className, string attributeName)
        {
            return node.Descendants(elementName)
                .FirstOrDefault(n => n.HasClass(className))
                ?.GetAttributeValue(attributeName, string.Empty) ?? string.Empty;
        }

        /// <summary>
        /// Extracts the last author's name from the publication HTML node
        /// </summary>
        /// <param name="node">The HTML node containing author information</param>
        /// <returns>The last author's name or empty string if not found</returns>
        private string ExtractLastAuthorText(HtmlNode node)
        {
            return node.Descendants("span")
                .Where(n => n.HasClass("highwire-citation-author"))
                .LastOrDefault()
                ?.InnerText
                ?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Enriches a list of publications with their abstracts by fetching additional details
        /// </summary>
        /// <param name="publications">The list of publications to enrich</param>
        private async Task EnrichPublicationsWithAbstracts(List<Publication> publications)
        {
            var tasks = publications.Select(p => GetArticleDetailAsync(p));
            var articleDetails = await Task.WhenAll(tasks);

            foreach (var publication in publications)
            {
                publication.Abstract = articleDetails
                    .FirstOrDefault(a => string.Equals(a.Doi, publication.DOI, StringComparison.InvariantCultureIgnoreCase))
                    ?.Abstract;
            }
        }

        /// <summary>
        /// Removes the schema prefix from a DOI string
        /// </summary>
        /// <param name="text">The DOI text to process</param>
        /// <returns>The cleaned DOI string</returns>
        private string RemoveSchema(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            var doi = text.Substring(4).Trim(); // remove "doi:"
            return doi.StartsWith(DOI_PREFIX) ? doi.Substring(DOI_PREFIX.Length) : doi;
        }

        /// <summary>
        /// Retrieves detailed article information from the BioRxiv API
        /// </summary>
        /// <param name="publication">The publication to fetch details for</param>
        /// <returns>Detailed article information</returns>
        private async Task<BioRxivArticleDetail> GetArticleDetailAsync(Publication publication)
        {
            ArgumentNullException.ThrowIfNull(publication);
            ArgumentException.ThrowIfNullOrEmpty(publication.DOI);

            string url = $"{API_URL}/details/biorxiv/{publication.DOI}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadFromJsonAsync<BioRxivArticlesResult>();

            var article = res.ArticleDetails.OrderBy(b => b.Version).Select(b => b).LastOrDefault();

            return article;
        }

        /// <summary>
        /// Converts a string date to DateOnly format
        /// </summary>
        /// <param name="dateString">The date string to convert (expected format: YYYY.MM.DD)</param>
        /// <returns>A DateOnly object or null if parsing fails</returns>
        private DateOnly? FormatToDateOnly(string dateString)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(dateString);

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

        /// <summary>
        /// Builds the search query URL with the provided parameters
        /// </summary>
        /// <param name="query">Search parameters to be converted into URL format</param>
        /// <returns>URL-encoded search query string</returns>
        public string BuildSearchQuery(LiteratureSearchQuery query)
        {
            ArgumentNullException.ThrowIfNull(query);

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

            // Add mandatory parameters
            queryParameters.Add("jcode%3Abiorxiv");
            queryParameters.Add($"numresults%3A{query.NbItems}");
            queryParameters.Add("format_result%3Astandard");

            if (query.Sort == SortBy.Date)
            {
                queryParameters.Add("sort%3Adate");
            }
            else
            {
                queryParameters.Add("sort%3Arelevance-rank");
            }

            // Add recent filter if specified
            if (query.Prioritze == Prioritze.Recent)
            {
                queryParameters.Add($"filter%3Adate-from%3A{DateTime.UtcNow.AddYears(-1):yyyy-MM-dd}");
            }

            // Join all parameters into the final URL
            string queryString = string.Join("%20", queryParameters);

            return queryString;
        }
    }
}
