using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;

namespace Witivio.Copilot4Researcher.Features.Collaboration.Models
{



    public class CollaborationQuery
    {
        public string[] Keywords { get; set; }

        public string UserIntent { get; set; }

        public string UserInput { get; set; }
    }

    public class CollaborationInputQuery
    {
        [Required]
        [SwaggerSchema(Description = "User input as a natural language query, e.g., 'Who does western blots?'")]
        public string UserInput { get; set; }

        [Required]
        [SwaggerSchema(Description = "User's intent derived from the natural language input, e.g., 'search for people.'")]
        public string UserIntent { get; set; }

        [Required]
        [SwaggerSchema(Description = "Keywords for what. Exclude action words. The keywords will be uses to search in a product database. Correct spelling if needed. JUST RESPOND WITH KEYWORDS SEPARATED BY semicolons. Respond in english")]
        public string Keywords { get; set; }

        public CollaborationQuery ToQuery()
        {
            return new CollaborationQuery
            {
                UserInput = UserInput,
                UserIntent = UserIntent,
                Keywords = Keywords.Split(";")
            };
        }
    }


}
