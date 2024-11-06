using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Providers.Pubmed.Models
{
    public class Header
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }


}
