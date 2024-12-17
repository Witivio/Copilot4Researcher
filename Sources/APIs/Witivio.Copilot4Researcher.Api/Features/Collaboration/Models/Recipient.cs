using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Features.Collaboration.Models
{
    public class Recipient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        

        [JsonPropertyName("unitName")]
        public string UnitName { get; set; }

        public int DeliveryId { get; set; }

        public DeliveryNote Delivery { get; set; }

    }

}
