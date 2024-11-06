using System.ComponentModel;

namespace Witivio.Copilot4Researcher.Models
{
    public class ClinicalTrial
    {
        public string Status { get; set; }
        public string Title { get; set; }
        public string NctId { get; set; }
        public List<string> Conditions { get; set; }
        public string SponsorAffiliation { get; set; }
        public string StartDate { get; set; }
        public string CompletionDate { get; set; }
        public int EnrollmentCount { get; set; }
        public List<string> Phases { get; set; }
        public List<Intervention> Interventions { get; set; }
        public List<ClinicalTrialContact> Contacts { get; set; }
        public string Summary { get; set; }

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