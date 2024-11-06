using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Providers.Pubmed.Models
{
    public class PubmedSearchResult
    {
        [JsonPropertyName("header")]
        public Header Header { get; set; }

        [JsonPropertyName("esearchresult")]
        public ESearchResult ESearchResult { get; set; }
    }


}
