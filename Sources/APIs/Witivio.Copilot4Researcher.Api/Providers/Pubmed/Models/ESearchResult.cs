using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Providers.Pubmed.Models
{
    public class ESearchResult
    {
        [JsonPropertyName("count")]
        public string Count { get; set; }

        [JsonPropertyName("retmax")]
        public string Retmax { get; set; }

        [JsonPropertyName("retstart")]
        public string Retstart { get; set; }

        [JsonPropertyName("idlist")]
        public List<string> Idlist { get; set; }

        [JsonPropertyName("translationset")]
        public List<Translationset> Translationset { get; set; }

        [JsonPropertyName("querytranslation")]
        public string Querytranslation { get; set; }
    }


}
