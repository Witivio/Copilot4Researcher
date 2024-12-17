using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Features.Literature.Pubmed.Models
{

    public class Translationset
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("to")]
        public string To { get; set; }
    }


}
