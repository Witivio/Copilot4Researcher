using Witivio.Copilot4Researcher.Models;
using static System.Net.WebRequestMethods;

namespace Witivio.Copilot4Researcher.Providers.ClinicalTrials
{
    public class ClinicalTrialsClient : IClinicalTrialsClient
    {
        private readonly HttpClient _httpClient;

        public async Task<IEnumerable<ClinicalTrial>> SearchAsync(ClinicalTrialSearchQuery query)
        {

            HttpResponseMessage response = await _httpClient.GetAsync($"studies?{BuildSearchQuery(query)}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ClinicalTrialSearchResult>();

            return result.Studies.Select(s => new ClinicalTrial
            {
                Status = s.ProtocolSection?.StatusModule?.OverallStatus,
                Title = s.ProtocolSection?.IdentificationModule?.OfficialTitle,
                NctId = s.ProtocolSection?.IdentificationModule?.NctId,
                Conditions = s.ProtocolSection?.ConditionsModule?.Conditions,
                SponsorAffiliation = s.ProtocolSection?.SponsorCollaboratorsModule?.ResponsibleParty?.InvestigatorAffiliation,
                StartDate = s.ProtocolSection?.StatusModule?.StartDateStruct?.Date,
                CompletionDate = s.ProtocolSection?.StatusModule?.CompletionDateStruct?.Date,
                EnrollmentCount = s.ProtocolSection?.DesignModule?.EnrollmentInfo?.Count ?? 0,
                Phases = s.ProtocolSection?.DesignModule?.Phases,
                Interventions = s.ProtocolSection?.ArmsInterventionsModule?.Interventions?.Select(i => new Models.Intervention
                {
                    Type = i.Type,
                    Name = i.Name
                }).ToList(),
                Contacts = s.ProtocolSection?.ContactsLocationsModule?.CentralContacts?.Select(c => new Models.ClinicalTrialContact
                {
                    Name = c.Name,
                    Email = c.Email,
                    Phone = c.Phone
                }).ToList(),
                Summary = s.ProtocolSection?.DescriptionModule?.BriefSummary,
                Link = $"https://www.clinicaltrials.gov/study/{s.ProtocolSection?.IdentificationModule.NctId}"
            });
        }

        public ClinicalTrialsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://clinicaltrials.gov/api/v2/");
        }

        private string BuildSearchQuery(ClinicalTrialSearchQuery query)
        {
            var builder = System.Web.HttpUtility.ParseQueryString(string.Empty);

            builder["query.term"] = string.Join(" ", query.Keywords);

            builder["pageSize"] = query.NbItems.ToString();

            builder["sort"] = "@relevance";

            if (!string.IsNullOrWhiteSpace(query.Conditions))
                builder["query.cond"] = query.Conditions;

            if (!string.IsNullOrWhiteSpace(query.Intervention))
                builder["query.intr"] = query.Intervention;

            if (!string.IsNullOrWhiteSpace(query.Id))
                builder["query.id"] = query.Id;

            if (!string.IsNullOrWhiteSpace(query.Sponsor))
                builder["query.spons"] = query.Sponsor;

            if (!string.IsNullOrWhiteSpace(query.LeadSponsorName))
                builder["query.lead"] = query.LeadSponsorName;

            if (!string.IsNullOrWhiteSpace(query.OutcomeMeasure))
                builder["query.outc"] = query.OutcomeMeasure;


            return builder.ToString();
        }

    }

    public interface IClinicalTrialsClient
    {
        Task<IEnumerable<ClinicalTrial>> SearchAsync(ClinicalTrialSearchQuery query);
    }
}

