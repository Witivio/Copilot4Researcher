using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Features.Literature.Pubmed.Models
{
    public class PubmedSearchResult
    {
        [JsonPropertyName("header")]
        public Header Header { get; set; }

        [JsonPropertyName("esearchresult")]
        public ESearchResult ESearchResult { get; set; }
    }


}
