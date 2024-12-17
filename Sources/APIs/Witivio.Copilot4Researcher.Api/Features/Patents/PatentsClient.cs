using System.Web;
using Witivio.Copilot4Researcher.Features.Patents.Models;

namespace Witivio.Copilot4Researcher.Features.Patents
{
    public interface IPatentsClient
    {
        Task<IEnumerable<Patent>> SearchAsync(PatentSearchQuery query);
    }

    public class PatentsClient : IPatentsClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public PatentsClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["SerpApi:ApiKey"]
                ?? throw new ArgumentNullException("SerpApi:ApiKey configuration is missing");
        }

        public async Task<IEnumerable<Patent>> SearchAsync(PatentSearchQuery query)
        {
            var queryString = BuildSearchQuery(query);

            var url = $"https://serpapi.com/search?engine=google_patents&{queryString}&api_key={_apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<PatentResult>();

            var patents = new List<Patent>();
            foreach (var item in result.OrganicResults)
            {
                var patent = new Patent
                {
                    Assignee = item.Assignee,
                    GooglePatentUrl = "https://patents.google.com/" + item.PatentId,
                    Title = item.Title.Replace("( ...", ""),
                    Summary = item.Snippet,
                    Inventor = item.Inventor,
                    PDFUrl = item.Pdf,
                    WIPO = item.PublicationNumber,
                    PatentScopeUrl = "https://patentscope.wipo.int/search/en/detail.jsf?docId=" + item.PublicationNumber,
                    Countries = item.CountryStatus?.Select(c => new Models.StatusByCountry { Name = c.Key, Status = Enum.Parse<PatentStatus>(c.Value) })
                };

                if (item.GrantDate != null)
                    patent.GrantDate = DateOnly.Parse(item.GrantDate);
                if (item.PriorityDate != null)
                    patent.PriorityDate = DateOnly.Parse(item.PriorityDate);
                if (item.PublicationDate != null)
                    patent.PublicationDate = DateOnly.Parse(item.PublicationDate);

                patents.Add(patent);
            }

            return patents;
        }

        private object BuildSearchQuery(PatentSearchQuery query)
        {
            var builder = HttpUtility.ParseQueryString(string.Empty);

            // Add keywords
            if (query.Keywords != null && query.Keywords.Any())
            {
                builder["q"] = string.Join(" ", query.Keywords);
            }

            // Add keywords
            if (query.Countries != null && query.Countries.Any())
            {
                builder["country"] = string.Join(",", query.Countries).ToUpper();
            }

            // Add language filter
            if (!string.IsNullOrEmpty(query.Language))
            {
                builder["language"] = query.Language.ToUpper();
            }

            // Add date filters
            if (!string.IsNullOrEmpty(query.BeforePriorityDate))
            {
                builder["before"] = "priority:" + query.BeforePriorityDate;
            }

            if (!string.IsNullOrEmpty(query.BeforeFillingDate))
            {
                builder["before"] = "filing:" + query.BeforeFillingDate;
            }

            if (!string.IsNullOrEmpty(query.BeforePublicationDate))
            {
                builder["before"] = "publication:" + query.BeforePublicationDate;
            }

            if (!string.IsNullOrEmpty(query.AfterPriorityDate))
            {
                builder["after"] = "priority:" + query.AfterPriorityDate;
            }

            if (!string.IsNullOrEmpty(query.AfterFillingDate))
            {
                builder["after"] = "filing:" + query.AfterFillingDate;
            }

            if (!string.IsNullOrEmpty(query.AfterPublicationDate))
            {
                builder["after"] = "publication:" + query.AfterPublicationDate;
            }

            // Add inventor and assignee filters
            if (!string.IsNullOrEmpty(query.Inventors))
            {
                builder["inventor"] = query.Inventors;
            }

            if (!string.IsNullOrEmpty(query.Assignees))
            {
                builder["assignee"] = query.Assignees;
            }

            // Add status filter
            if (!string.IsNullOrEmpty(query.Status))
            {
                builder["status"] = query.Status.ToUpper();
            }

            // Add type filter
            if (!string.IsNullOrEmpty(query.Type))
            {
                builder["type"] = query.Type.ToUpper();
            }

            // Add litigation filter
            if (!string.IsNullOrEmpty(query.Litigation))
            {
                builder["litigation"] = query.Litigation.ToUpper();
            }




            return builder.ToString();
        }
    }
}
