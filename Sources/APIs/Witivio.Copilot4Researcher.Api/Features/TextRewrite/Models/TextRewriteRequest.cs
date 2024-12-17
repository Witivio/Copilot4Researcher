using Witivio.Copilot4Researcher.Features.TextRewrite.Models;
using Witivio.Copilot4Researcher.Features.TextRewrite;

namespace Witivio.Copilot4Researcher.Features.TextRewrite.Models
{
    public class TextRewriteQuery
    {
        public string JournalName { get; set; }
        public TextType TextType { get; set; } = TextType.Abstract;
    }
}