using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Features.Collaboration.Models
{
    public class DeliveryNote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonPropertyName("date")]
        public DateOnly? Date { get; set; }
        [JsonPropertyName("number")]
        public string Number { get; set; }

        public int DeliveryNotesScanFileId { get; set; }
        public DeliveryNotesScanFile DeliveryNotesScanFile { get; set; }

        [JsonPropertyName("providerName")]
        public string ProviderName { get; set; }

        public int RecipientId { get; set; }
        [JsonPropertyName("recipient")]
        public Recipient Recipient { get; set; }
        [JsonPropertyName("products")]
        public List<Product> Products { get; set; }
    }

}
