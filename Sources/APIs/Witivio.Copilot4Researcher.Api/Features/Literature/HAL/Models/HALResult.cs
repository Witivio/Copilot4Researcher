using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Features.Literature.HAL.Models
{

    public class Doc
    {
        [JsonPropertyName("title_s")]
        public List<string> TitleS { get; set; }

        [JsonPropertyName("abstract_s")]
        public List<string> AbstractS { get; set; }

        [JsonPropertyName("authFullName_s")]
        public List<string> AuthFullNameS { get; set; }

        [JsonPropertyName("uri_s")]
        public string UriS { get; set; }

        [JsonPropertyName("doiId_s")]
        public string DoiIdS { get; set; }

        [JsonPropertyName("producedDate_s")]
        public string ProducedDateS { get; set; }

        [JsonPropertyName("journalTitle_s")]
        public string JournalTitleS { get; set; }

        [JsonPropertyName("halId_s")]
        public string HALId { get; set; }


    }

    public class HALResponse
    {
        [JsonPropertyName("numFound")]
        public int NumFound { get; set; }

        [JsonPropertyName("start")]
        public int Start { get; set; }

        [JsonPropertyName("maxScore")]
        public double MaxScore { get; set; }

        [JsonPropertyName("numFoundExact")]
        public bool NumFoundExact { get; set; }

        [JsonPropertyName("docs")]
        public List<Doc> Docs { get; set; }
    }

    public class HALResult
    {
        [JsonPropertyName("response")]
        public HALResponse Response { get; set; }
    }


}
