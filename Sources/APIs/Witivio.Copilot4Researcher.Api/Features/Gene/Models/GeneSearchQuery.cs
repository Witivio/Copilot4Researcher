using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Witivio.Copilot4Researcher.Features.Gene.Models
{
    public class GeneSearchQuery
    {
        [Required]
        [SwaggerSchema(Description = "Name of a gene to search, e.g., BRCA1 or KNG1")]
        public string Name { get; set; }
    }

}
