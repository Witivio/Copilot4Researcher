using Witivio.Copilot4Researcher.Features.Patents;

namespace Witivio.Copilot4Researcher.Features.Patents.Models
{
    public class Patent
    {
        public string Title { get; set; }
        public string WIPO { get; set; }
        public string Inventor { get; set; }
        public string Assignee { get; set; }

        public IEnumerable<StatusByCountry> Countries { get; set; }

        public DateOnly? PriorityDate { get; set; }
        public DateOnly? GrantDate { get; set; }
        public DateOnly? PublicationDate { get; set; }

        public string Summary { get; set; }
        public string GooglePatentUrl { get; set; }
        public string PatentScopeUrl { get; set; }
        public string PDFUrl { get; set; }
    }

}
