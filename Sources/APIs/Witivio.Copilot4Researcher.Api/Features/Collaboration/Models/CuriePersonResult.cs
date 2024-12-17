namespace Witivio.Copilot4Researcher.Api.Features.Collaboration.Models;

public record CuriePersonResult
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string PhotoUrl { get; init; }

}

