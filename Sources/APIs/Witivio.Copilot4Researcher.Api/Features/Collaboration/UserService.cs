using Witivio.Copilot4Researcher.Features.Collaboration.Models;

namespace Witivio.Copilot4Researcher.Api.Features.Collaboration;

public interface IUserService
{
    Task<UserDetails> SearchAsync(string fullName, CancellationToken cancellationToken = default);
}

public class UserService : IUserService
{
    private readonly ICurieDirectoryService _curieDirectoryService;
    private readonly IEntraIDUserService _entraIdUserService;
    private readonly ILogger<UserService> _logger;

    public UserService(
        ICurieDirectoryService curieDirectoryService,
        IEntraIDUserService entraIdUserService,
        ILogger<UserService> logger)
    {
        _curieDirectoryService = curieDirectoryService ?? throw new ArgumentNullException(nameof(curieDirectoryService));
        _entraIdUserService = entraIdUserService ?? throw new ArgumentNullException(nameof(entraIdUserService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<UserDetails> SearchAsync(string fullName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);

        try
        {
            // First, search in Curie Directory
            var curieResults = await _curieDirectoryService.SearchByNameAsync(fullName, cancellationToken);
            var curieMatch = curieResults?.FirstOrDefault();

            if (curieMatch is null)
            {
                _logger.LogInformation("No matching user found in Curie Directory for {fullName}",
                    fullName);
                return null;
            }

            var entraIdResults = await _entraIdUserService.SearchUsersByNameAsync(curieMatch.FirstName, curieMatch.LastName, cancellationToken);
            var entraIdMatch = entraIdResults.FirstOrDefault();

            if (entraIdMatch is null)
            {
                _logger.LogInformation("No matching user found in EntraID for {FirstName} {LastName}",
                    curieMatch.FirstName, curieMatch.LastName);
                return null;
            }

            // If EntraID has no photo, use Curie photo
            if (string.IsNullOrEmpty(entraIdMatch.PhotoUrl) && !string.IsNullOrEmpty(curieMatch.PhotoUrl))
            {
                entraIdMatch.PhotoUrl = curieMatch.PhotoUrl;
            }

            return entraIdMatch;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching for user {fullName}", fullName);
            throw;
        }
    }
} 