using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Features.Literature.Models
{

    public class Publication
    {
        [JsonIgnore]
        public string Id { get; set; }

        [JsonIgnore]
        public string DOI { get; set; }


        public string Abstract { get; set; }

        [JsonIgnore]
        public Authors Authors { get; set; }


        public DateOnly? Date { get; set; }


        public string Title { get; set; }


        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PublicationSource Source { get; set; }

        public enum PublicationSource
        {
            Pubmed,
            HAL,
            BioRxiv
        }


        public string Link { get; set; }

        [JsonIgnore]
        public int? Citations { get; set; }

        [JsonIgnore]
        public double? ImpactFactor { get; set; }

        [JsonIgnore]
        public string JournalName { get; set; }


    }

}
