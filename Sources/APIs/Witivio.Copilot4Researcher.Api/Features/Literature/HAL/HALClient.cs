using HtmlAgilityPack;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Witivio.Copilot4Researcher.Core;
using Witivio.Copilot4Researcher.Features.Literature.HAL.Models;
using Witivio.Copilot4Researcher.Features.Literature.Models;

namespace Witivio.Copilot4Researcher.Features.Literature.HAL
{
    public interface IHALClient
    {
        Task<IEnumerable<Publication>> SearchAsync(LiteratureSearchQuery query);
    }

    public class HALCLient : IHALClient
    {
        private readonly HttpClient _httpClient;

        public HALCLient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://api.archives-ouvertes.fr");
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Publication>> SearchAsync(LiteratureSearchQuery query)
        {

            try
            {
                return await SearchInternalAsync(query);
            }
            catch (Exception)
            {

                return Enumerable.Empty<Publication>();
            }
        }

        private async Task<IEnumerable<Publication>> SearchInternalAsync(LiteratureSearchQuery query)
        {

            var queryString = BuildSearchQuery(query);

            var url = $"/search?{queryString}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<HALResult>();

            var publications = new List<Publication>();
            foreach (var doc in result.Response.Docs)
            {
                var publication = new Publication
                {
                    Title = doc.TitleS.First(),
                    JournalName = doc.JournalTitleS,
                    Abstract = doc.AbstractS.FirstOrDefault(),
                    Authors = new Authors
                    {
                        First = doc.AuthFullNameS?.First(),
                        Last = doc.AuthFullNameS?.Last()
                    },
                    DOI = doc.DoiIdS,
                    Link = "https://hal.science/" + doc.HALId,
                    Source = Publication.PublicationSource.HAL,
                    Date = DateOnly.TryParse(doc.ProducedDateS, out var date) ? date : null
                };
                publications.Add(publication);
            }
            return publications;
        }

        private string BuildSearchQuery(LiteratureSearchQuery query)
        {
            var builder = HttpUtility.ParseQueryString(string.Empty);

            builder["wt"] = "json";
            builder["rows"] = query.NbItems.ToString();
            builder["fl"] = "authFullName_s,doiId_s,journalTitle_s,title_s,uri_s,producedDate_s,abstract_s,halId_s";
            builder["fq"] = "docType_s:ART"; // filter for article only

            // Add keywords
            if (query.Keywords != null && query.Keywords.Any())
            {
                builder["q"] = string.Join(" ", query.Keywords);
            }

            // Add authors
            if (query.Authors != null && query.Authors.Any())
            {
                builder["q"] += string.Join(" ", query.Authors);
            }

            if (query.Prioritze == Prioritze.Recent)
            {
                builder["fq"] += " AND producedDateY_i:[" + DateTime.UtcNow.AddYears(-1).Year + " TO *]";
            }

            // Add sort parameter
            builder["sort"] = query.Sort == SortBy.Date ? "producedDate_s desc" : "score desc";

            return builder.ToString();
        }
    }
}
