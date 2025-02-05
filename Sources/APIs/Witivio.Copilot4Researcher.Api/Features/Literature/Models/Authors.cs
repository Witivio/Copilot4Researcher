using Swashbuckle.AspNetCore.Annotations;

namespace Witivio.Copilot4Researcher.Features.Literature.Models
{

    /// <summary>
    /// Represents an author of a publication.
    /// </summary>
    [SwaggerSchema(Description = "Represents the details of an author of a publication, including their first and last name.")]
    public class Authors
    {
        [SwaggerSchema(Description = "The first name of the author.")]
        public string First { get; set; }

        [SwaggerSchema(Description = "The last name of the author.")]
        public string Last { get; set; }
    }

}
