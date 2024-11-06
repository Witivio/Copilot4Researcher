using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Providers.Pubmed.Models
{
    public class Linksetdb
    {
        [JsonPropertyName("dbto")]
        public string Dbto { get; set; }

        [JsonPropertyName("linkname")]
        public string Linkname { get; set; }

        [JsonPropertyName("links")]
        public List<string> Links { get; set; }
    }


}
