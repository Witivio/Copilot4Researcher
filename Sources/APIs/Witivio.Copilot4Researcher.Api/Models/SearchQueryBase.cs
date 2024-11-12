namespace Witivio.Copilot4Researcher.Models
{
    public abstract class SearchQueryBase
    {
        public string UserInput { get; set; }
        
        public string UserIntent { get; set; }
        
        public int NbItems { get; set; } = 5;

     

        public SortBy Sort { get; set; } = SortBy.Relevance;
    }

    public enum SortBy
    {
        Relevance,
        Date
    }
}