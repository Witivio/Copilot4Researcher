using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Providers.Pubmed.Models
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);


    public class Linkset
    {
        [JsonPropertyName("dbfrom")]
        public string Dbfrom { get; set; }

        [JsonPropertyName("ids")]
        public List<string> Ids { get; set; }

        [JsonPropertyName("linksetdbs")]
        public List<Linksetdb> Linksetdbs { get; set; }
    }


}
