using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace Witivio.Copilot4Researcher.Models
{
    public class ClinicalTrialSearchQuery
    {
        public string[] Keywords { get; set; }

        public string UserIntent { get; set; }

        public string UserInput { get; set; }

        public int NbItems { get; set; } = 5;
        public string Conditions { get; set; }

        public string Intervention { get; set; }

        public string OutcomeMeasure { get; set; }

        public string Sponsor { get; set; } = "Institut Curie";

        public string LeadSponsorName { get; set; }

        public string Id { get; set; }
    }

    public class ClinicalTrialInputSearchQuery
    {
        [Required]
        [SwaggerSchema(Description = "User input as a natural language query, e.g., 'Show latest articles about BRCA1.'")]
        public string UserInput { get; set; }

        [Required]
        [SwaggerSchema(Description = "User's intent derived from the natural language input, e.g., 'search for recent studies.'")]
        public string UserIntent { get; set; }

        [Required]
        [SwaggerSchema(Description = "Keywords or topics to focus the search on, extracted from the user input, separated by semicolons.")]
        public string Keywords { get; set; }

        [SwaggerSchema(Description = "Optional. Number of items to return. Default value is 5")]
        public int NbItems { get; set; } = 5;

        [SwaggerSchema(Description = "Optional. Conditions or disease in Essie expression syntax")]
        public string Conditions { get; set; }

        [SwaggerSchema(Description = "Optional. Intervention or treatment in Essie expression syntax")]
        public string Intervention { get; set; }

        [SwaggerSchema(Description = "Optional. Outcom measure in Essie expression syntax")]
        public string OutcomeMeasure { get; set; }

        [SwaggerSchema(Description = "Optional. Sponsor or collaborator lead in Essie expression syntax")]
        public string LeadSponsorName { get; set; }

        [SwaggerSchema(Description = "Optional. NCT Number of a study")]
        public string Id { get; set; }

        public ClinicalTrialSearchQuery ToQuery()
        {
            var result = new ClinicalTrialSearchQuery
            {
                UserInput = this.UserInput,
                UserIntent = this.UserIntent,
                NbItems = this.NbItems,
                Keywords = this.Keywords?.Split(";"),
                Conditions = this.Conditions,
                Intervention = this.Intervention,
                OutcomeMeasure = this.OutcomeMeasure,
                LeadSponsorName = this.LeadSponsorName,
                Id = this.Id
            };
            return result;
        }

    }


}
