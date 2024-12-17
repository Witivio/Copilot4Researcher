namespace Witivio.Copilot4Researcher.Features.TextRewrite.Models
{
    /// <summary>
    /// Represents text analysis rules for a journal
    /// </summary>
    public record TextAnalysisRule
    {

        public string WordCount { get; init; }

        public string JournalName { get; set; }
    }
}