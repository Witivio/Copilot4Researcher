using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Providers.Patents.Models
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class Assignee
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("percentage")]
        public double Percentage { get; set; }

        [JsonPropertyName("frequency")]
        public List<Frequency> Frequency { get; set; }
    }

    public class CountryStatus
    {
        [JsonPropertyName("US")]
        public string US { get; set; }

        [JsonPropertyName("WO")]
        public string WO { get; set; }

        [JsonPropertyName("EP")]
        public string EP { get; set; }

        [JsonPropertyName("CN")]
        public string CN { get; set; }

        [JsonPropertyName("JP")]
        public string JP { get; set; }

        [JsonPropertyName("KR")]
        public string KR { get; set; }

        [JsonPropertyName("AU")]
        public string AU { get; set; }

        [JsonPropertyName("BR")]
        public string BR { get; set; }

        [JsonPropertyName("CA")]
        public string CA { get; set; }

        [JsonPropertyName("CO")]
        public string CO { get; set; }

        [JsonPropertyName("CR")]
        public string CR { get; set; }

        [JsonPropertyName("DK")]
        public string DK { get; set; }

        [JsonPropertyName("EC")]
        public string EC { get; set; }

        [JsonPropertyName("ES")]
        public string ES { get; set; }

        [JsonPropertyName("MX")]
        public string MX { get; set; }

        [JsonPropertyName("PT")]
        public string PT { get; set; }

        [JsonPropertyName("ZA")]
        public string ZA { get; set; }

        [JsonPropertyName("IN")]
        public string IN { get; set; }

        [JsonPropertyName("RU")]
        public string RU { get; set; }

        [JsonPropertyName("CL")]
        public string CL { get; set; }

        [JsonPropertyName("PH")]
        public string PH { get; set; }
    }

    public class Cpc
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("percentage")]
        public double Percentage { get; set; }

        [JsonPropertyName("frequency")]
        public List<Frequency> Frequency { get; set; }
    }

    public class Figure
    {
        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonPropertyName("full")]
        public string Full { get; set; }
    }

    public class Frequency
    {
        [JsonPropertyName("year_range")]
        public string YearRange { get; set; }

        [JsonPropertyName("percentage")]
        public double Percentage { get; set; }
    }

    public class Inventor
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("percentage")]
        public double Percentage { get; set; }

        [JsonPropertyName("frequency")]
        public List<Frequency> Frequency { get; set; }
    }

    
    public class OrganicResult
    {
        [JsonPropertyName("position")]
        public int Position { get; set; }

        [JsonPropertyName("rank")]
        public int Rank { get; set; }

        [JsonPropertyName("patent_id")]
        public string PatentId { get; set; }

        [JsonPropertyName("serpapi_link")]
        public string SerpapiLink { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("snippet")]
        public string Snippet { get; set; }

        [JsonPropertyName("priority_date")]
        public string PriorityDate { get; set; }

        [JsonPropertyName("filing_date")]
        public string FilingDate { get; set; }

        [JsonPropertyName("publication_date")]
        public string PublicationDate { get; set; }

        [JsonPropertyName("inventor")]
        public string Inventor { get; set; }

        [JsonPropertyName("assignee")]
        public string Assignee { get; set; }

        [JsonPropertyName("publication_number")]
        public string PublicationNumber { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonPropertyName("pdf")]
        public string Pdf { get; set; }

        [JsonPropertyName("figures")]
        public List<Figure> Figures { get; set; }

        [JsonPropertyName("country_status")]
        public Dictionary<string, string> CountryStatus { get; set; }

        [JsonPropertyName("grant_date")]
        public string GrantDate { get; set; }
    }

    public class Pagination
    {
        [JsonPropertyName("current")]
        public int Current { get; set; }

        [JsonPropertyName("next")]
        public string Next { get; set; }
    }

    public class PatentResult
    {
        [JsonPropertyName("search_metadata")]
        public SearchMetadata SearchMetadata { get; set; }

        [JsonPropertyName("search_parameters")]
        public SearchParameters SearchParameters { get; set; }

        [JsonPropertyName("search_information")]
        public SearchInformation SearchInformation { get; set; }

        [JsonPropertyName("organic_results")]
        public List<OrganicResult> OrganicResults { get; set; }

        [JsonPropertyName("summary")]
        public Summary Summary { get; set; }

        [JsonPropertyName("pagination")]
        public Pagination Pagination { get; set; }

        [JsonPropertyName("serpapi_pagination")]
        public SerpapiPagination SerpapiPagination { get; set; }
    }

    public class SearchInformation
    {
        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("page_number")]
        public int PageNumber { get; set; }
    }

    public class SearchMetadata
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("json_endpoint")]
        public string JsonEndpoint { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("processed_at")]
        public string ProcessedAt { get; set; }

        [JsonPropertyName("google_patents_url")]
        public string GooglePatentsUrl { get; set; }

        [JsonPropertyName("raw_html_file")]
        public string RawHtmlFile { get; set; }

        [JsonPropertyName("prettify_html_file")]
        public string PrettifyHtmlFile { get; set; }

        [JsonPropertyName("total_time_taken")]
        public double TotalTimeTaken { get; set; }
    }

    public class SearchParameters
    {
        [JsonPropertyName("engine")]
        public string Engine { get; set; }

        [JsonPropertyName("q")]
        public string Q { get; set; }

        [JsonPropertyName("num")]
        public string Num { get; set; }
    }

    public class SerpapiPagination
    {
        [JsonPropertyName("current")]
        public int Current { get; set; }

        [JsonPropertyName("next")]
        public string Next { get; set; }
    }

    public class Summary
    {
        [JsonPropertyName("assignee")]
        public List<Assignee> Assignee { get; set; }

        [JsonPropertyName("inventor")]
        public List<Inventor> Inventor { get; set; }

        [JsonPropertyName("cpc")]
        public List<Cpc> Cpc { get; set; }
    }


}
