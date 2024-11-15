using Witivio.Copilot4Researcher.Api.Providers;

namespace Witivio.Copilot4Researcher.Api.Models
{
    public class TextRewriteQuery
    {
        public string JournalName { get; set; }
        public string DocumentType { get; set; }
        public string InputText { get; set; }
        public JournalType JournalType { get; internal set; }
    }
}