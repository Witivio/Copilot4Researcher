using HtmlAgilityPack;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Witivio.Copilot4Researcher.Features.Gene.Models;
using Witivio.Copilot4Researcher.Features.Gene.ProteinAtlas.Models;

namespace Witivio.Copilot4Researcher.Features.Gene.ProteinAtlas
{
    public interface IProteinAtlasClient
    {
        Task<Witivio.Copilot4Researcher.Features.Gene.Models.Gene> SearchAsync(GeneSearchQuery query);
    }

    public class ProteinAtlasClient : IProteinAtlasClient
    {
        private readonly HttpClient _httpClient;

        public ProteinAtlasClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Witivio.Copilot4Researcher.Features.Gene.Models.Gene> SearchAsync(GeneSearchQuery query)
        {
            var ensembl = await GetEnsemblAsync(query);
            if (ensembl == null)
                return null;
            var result = await GetEnsemblDetailAsync(ensembl);
            var summaryResult = await GetEnsembleSummaryAsync(ensembl);

            var gene = new Witivio.Copilot4Researcher.Features.Gene.Models.Gene
            {
                Name = result.Entry.Name,
                GeneDescription = summaryResult.GeneDescription,
                GeneName = result.Entry.Name + " (" + string.Join(", ", result.Entry.Synonyms) + ")",
                ProteinClass = string.Join(", ", result.Entry.ProteinClasses.Where(p => p.Id == "Ca" || p.Id == "Dr" || p.Id == "Ez" || p.Id == "Ha" || p.Id == "Pp" || p.Id == "Pd").Select(p => p.Name)),
                Interactions = summaryResult.Interactions.ToString(),
                ProteinAtlasLink = $"https://www.proteinatlas.org/{ensembl.Value}",
                UniprotLink = $"https://www.uniprot.org/uniprotkb/{summaryResult.Uniprot.FirstOrDefault()}",
                Expressions = result.Entry.TissueExpression?.Images.Select(p => new Witivio.Copilot4Researcher.Features.Gene.Models.TissueExpression
                {
                    Organ = p.Tissue.Name,
                    ImageUrl = p.ImageUrl
                }).ToList()
            };

            return gene;
        }

        private async Task<ProteinAtlasSummaryResult> GetEnsembleSummaryAsync(Ensembl ensembl)
        {
            string url = $"https://www.proteinatlas.org/{ensembl.Value}.json";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var res = await response.Content.ReadFromJsonAsync<ProteinAtlasSummaryResult>();
            return res;
        }

        public async Task<ProteinAtlasResult> GetEnsemblDetailAsync(Ensembl ensembl)
        {
            string url = $"https://www.proteinatlas.org/{ensembl.Value}.xml";


            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                // Deserialize the XML response into the ProteinAtlas object
                XmlSerializer serializer = new XmlSerializer(typeof(ProteinAtlasResult));

                using (StringReader textReader = new StringReader(responseBody))
                {
                    using (XmlTextReader reader = new XmlTextReader(textReader))
                    {
                        reader.Namespaces = false;
                        return (ProteinAtlasResult)serializer.Deserialize(reader);
                    }

                }
            }
            catch (Exception)
            {
                return null;

            }
        }

        private async Task<Ensembl> GetEnsemblAsync(GeneSearchQuery query)
        {
            string url = "https://www.proteinatlas.org/api/search_download.php?search=" + HttpUtility.UrlEncode(query.Name) + "&format=json&columns=g,gs,ectissue,up,eg&compress=no";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<SearchResult[]>();

            if (!result.Any())
                return null;

            return new Ensembl { Value = result[0].Ensembl };
        }
    }
}
