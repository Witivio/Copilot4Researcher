namespace Witivio.Copilot4Researcher.Core
{
    public abstract class SearchQueryBase
    {
        public string UserInput { get; set; }

        public string UserIntent { get; set; }

        public int NbItems { get; set; } = 5;

        public SortBy Sort { get; set; } = SortBy.Relevance;

        public Prioritze Prioritze { get; set; } = Prioritze.None;
    }

    public enum Prioritze
    {
        None,
        Recent
    }

    public enum SortBy
    {
        Relevance,
        Date
    }
}
