using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Witivio.Copilot4Researcher.Models
{
    public class LiteratureSearchQuery
    {

        
        
        public string UserInput { get; set; }

        
        
        public string UserIntent { get; set; }

        
        public int NbItems { get; set; } = 5;

        
        
        public string[] Keywords { get; set; }

        
        public DateTime? MinDate { get; set; }

        
        public DateTime? MaxDate { get; set; }

        
        public string[] Authors { get; set; }
    }

    public class LiteratureSearchInputQuery
    {
        [Required]
        [SwaggerSchema(Description = "User input as a natural language query, e.g., 'Show latest articles about BRCA1.'")]
        public string UserInput { get; set; }

        [Required]
        [SwaggerSchema(Description = "User's intent derived from the natural language input, e.g., 'search for recent studies.'")]
        public string UserIntent { get; set; }

        [Required]
        [SwaggerSchema(Description = "Keywords or topics to focus the search on, extracted from the user input, separated by semicolons , e.g., BRCA1;genetic mutations.")]
        public string Keywords { get; set; }

        [SwaggerSchema(Description = "Optional. Minimum publication date to filter the search results. Default value is null.")]
        public DateTime? MinDate { get; set; }

        [SwaggerSchema(Description = "Optional. Maximum publication date to filter the search results. Default value is null.")]
        public DateTime? MaxDate { get; set; }

        [SwaggerSchema(Description = "Optional Array of author names to narrow the search, separated by semicolons, e.g., John Doe;Jane Smith. Default value is null.")]
        public string Authors { get; set; }

        [SwaggerSchema(Description = "Optional. Number of items to return. Default value is 5")]
        public int NbItems { get; set; } = 5;

        public LiteratureSearchQuery ToQuery()
        {
            var result = new LiteratureSearchQuery
            {
                UserInput = this.UserInput,
                UserIntent = this.UserIntent,
                MaxDate = this.MaxDate,
                MinDate = this.MinDate,
                Authors = this.Authors?.Split(';'),
                Keywords = this.Keywords?.Split(";"),
                NbItems = this.NbItems

            };
            return result;
        }



    }

}
