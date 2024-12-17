using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Features.ClinicalTrials.Models
{
    public class ClinicalTrial
    {
        public string Status { get; set; }
        public string Title { get; set; }
        [JsonIgnore]
        public string NctId { get; set; }
        [JsonIgnore]
        public List<string> Conditions { get; set; }
        [JsonIgnore]
        public string SponsorAffiliation { get; set; }
        [JsonIgnore]
        public string StartDate { get; set; }
        [JsonIgnore]
        public string CompletionDate { get; set; }
        [JsonIgnore]
        public int EnrollmentCount { get; set; }
        [JsonIgnore]
        public List<string> Phases { get; set; }
        [JsonIgnore]
        public List<Intervention> Interventions { get; set; }
        [JsonIgnore]
        public List<ClinicalTrialContact> Contacts { get; set; }

        public string Summary { get; set; }
        [JsonIgnore]
        public string Link { get; set; }
    }

    public class Intervention
    {
        public string Type { get; set; }
        public string Name { get; set; }
    }

    public class ClinicalTrialContact
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}