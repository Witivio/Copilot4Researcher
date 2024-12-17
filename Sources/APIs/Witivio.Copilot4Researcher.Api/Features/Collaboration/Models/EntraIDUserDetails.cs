using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Features.Collaboration.Models
{
    public class UserDetails
    {
        public string FullName { get; init; }
        public string Location { get; init; }
        public string BusinessPhone { get; init; }
        public string JobTitle { get; init; }
        [JsonIgnore]
        public string PhotoUrl { get; set; }
        public string Mail { get; init; }
    }
}
