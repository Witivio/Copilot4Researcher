using AdaptiveCards;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Features.Gene.Models
{

    public class Gene
    {
        public string Name { get; set; }
        public string GeneDescription { get; set; }
        public string GeneName { get; set; }
        public string ProteinClass { get; set; }
        public string Transcripts { get; set; }
        public string Interactions { get; set; }
        [JsonIgnore]
        public string ProteinAtlasLink { get; set; }
        [JsonIgnore]
        public string UniprotLink { get; set; }
        [JsonIgnore]
        public IEnumerable<TissueExpression> Expressions { get; set; }


    }

}
