namespace Witivio.Copilot4Researcher.Core
{
    public class CopilotRenderingResultsWrapper<T>
    {
        public List<CopilotRenderingResult<T>> Results { get; set; }

        public CopilotResultTemplates Templates { get; set; }
    }

}
