using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Features.Collaboration.Models
{


    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }


        public NpgsqlTsVector SearchVector { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        public string Keywords { get; set; }

        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }

        public int DeliveryId { get; set; }

        public DeliveryNote Delivery { get; set; }
    }

}
