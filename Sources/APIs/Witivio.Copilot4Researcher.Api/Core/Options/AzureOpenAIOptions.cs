namespace Witivio.Copilot4Researcher.Api.Options;

public class AzureOpenAIOptions
{
    public const string SectionName = "AzureOpenAI";

    public string ChatDeploymentName { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
} 