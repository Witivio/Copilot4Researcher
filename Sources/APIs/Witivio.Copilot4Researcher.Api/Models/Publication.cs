using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Models
{
    
    public class Publication
    {
        public string Icon { get; set; } = "https://cdn.ncbi.nlm.nih.gov/coreutils/nwds/img/favicons/favicon.png";

        public string Id { get; set; }
        
        public string DOI { get; set; }

        
        public string Abstract { get; set; }

        
        public Authors Authors { get; set; }

        
        public DateOnly? Date { get; set; }

        
        public string Title { get; set; }

        
        public string Source { get; set; }

        
        public string Link { get; set; }

        
        public string Citations { get; set; }

        
        public double ImpactFactor { get; set; }

        
        public string JournalName { get; set; }

       
    }

}
