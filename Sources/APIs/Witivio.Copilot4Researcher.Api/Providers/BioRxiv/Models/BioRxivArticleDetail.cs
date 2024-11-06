using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Providers.BioRxiv.Models
{
    public class BioRxivArticleDetail
    {
        [JsonPropertyName("doi")]
        public string Doi { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("authors")]
        public string Authors { get; set; }

        [JsonPropertyName("author_corresponding")]
        public string AuthorCorresponding { get; set; }

        [JsonPropertyName("author_corresponding_institution")]
        public string AuthorCorrespondingInstitution { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("license")]
        public string License { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("jatsxml")]
        public string Jatsxml { get; set; }

        [JsonPropertyName("abstract")]
        public string Abstract { get; set; }

        [JsonPropertyName("published")]
        public string Published { get; set; }

        [JsonPropertyName("server")]
        public string Server { get; set; }
    }


}
