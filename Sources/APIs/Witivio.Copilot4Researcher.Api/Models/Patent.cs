namespace Witivio.Copilot4Researcher.Models
{
    public class Patent
    {
        public string Title { get; set; }
        public string WIPO { get; set; }
        public string Inventor { get; set; }
        public string Assignee { get; set; }

        public IEnumerable<CountryStatus> Countries { get; set; }

        public DateOnly? PriorityDate { get; set; }
        public DateOnly? GrantDate { get; set; }
        public DateOnly? PublicationDate { get; set; }

        public string Summary { get; set; }
        public string GooglePatentUrl { get; set; }
        public string PatentScopeUrl { get; set; }
        public string PDFUrl { get; set; }
    }

}
