using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Witivio.Copilot4Researcher.Features.Collaboration.Models
{
    public class DeliveryNotesScanFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FileName { get; set; }
        public int? TotalPages { get; set; }

        public IndexingStatus Status { get; set; }

        public List<DeliveryNote> DeliveryNotes { get; set; } = new();
        public string FullPath { get; internal set; }

        public DateTime IndexingDate { get; set; }
    }

}
