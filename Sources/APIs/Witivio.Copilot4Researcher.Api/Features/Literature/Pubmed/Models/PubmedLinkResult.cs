using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Features.Literature.Pubmed.Models
{

    public class PubmedLinkResult
    {
        [JsonPropertyName("header")]
        public Header Header { get; set; }

        [JsonPropertyName("linksets")]
        public List<Linkset> Linksets { get; set; }
    }


}
