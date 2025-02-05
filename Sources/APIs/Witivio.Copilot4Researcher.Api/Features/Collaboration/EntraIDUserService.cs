namespace Witivio.Copilot4Researcher.Api.Features.Collaboration
{
    using global::Witivio.Copilot4Researcher.Features.Collaboration.Models;
    using Microsoft.Graph;
    using Microsoft.Graph.Models;
    using Microsoft.Graph.Models.TermStore;
    using System.Text.Json.Serialization;



    public interface IEntraIDUserService
    {
        Task<List<UserDetails>> SearchUsersByNameAsync(string firstName, string lastName, CancellationToken cancellationToken = default);
    }

    public class EntraIDUserService : IEntraIDUserService
    {
        private readonly GraphServiceClient _graphServiceClient;
        private readonly ILogger<EntraIDUserService> _logger;

        public EntraIDUserService(
            GraphServiceClient graphServiceClient,
            ILogger<EntraIDUserService> logger)
        {
            _graphServiceClient = graphServiceClient ?? throw new ArgumentNullException(nameof(graphServiceClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<UserDetails>> SearchUsersByNameAsync(string firstName, string lastName, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(firstName);
            ArgumentException.ThrowIfNullOrEmpty(lastName);
            try
            {
                var users = await SearchUsersByTerm(firstName, lastName, cancellationToken);
                if (users?.Value == null) return null;

                var allUsers = new List<UserDetails>();

                foreach (var user in users.Value)
                {
                    var photoBase64 = await GetUserPhotoBase64Async(user.Id!, cancellationToken);
                    allUsers.Add(new UserDetails
                    {
                        FullName = user.DisplayName,
                        Location = user.OfficeLocation,
                        Mail = user.Mail,
                        BusinessPhone = user.BusinessPhones?.FirstOrDefault(),
                        JobTitle = user.JobTitle,
                        PhotoUrl = photoBase64
                    });
                }

                return allUsers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching for term '{firsName}' '{lastName}'", firstName, lastName);
                return null;
            }
        }

        private async Task<Microsoft.Graph.Models.UserCollectionResponse> SearchUsersByTerm(
            string firstName, string lastName,
            CancellationToken cancellationToken)
        {
            return await _graphServiceClient.Users.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Count = true;
                requestConfiguration.QueryParameters.Search = $"\"surname:{lastName}\" AND \"givenName:{firstName}\"";
                requestConfiguration.QueryParameters.Orderby = new[] { "displayName" };
                requestConfiguration.QueryParameters.Select = new[]
                {
                    "id",
                    "displayName",
                    "businessPhones",
                    "officeLocation",
                    "mail",
                    "jobTitle"
            };
                requestConfiguration.Headers.Add("ConsistencyLevel", "eventual");
            }, cancellationToken);
        }
        private async Task<string> GetUserPhotoBase64Async(string userId, CancellationToken cancellationToken)
        {
            try
            {
                using var photoStream = await _graphServiceClient.Users[userId].Photo.Content
                    .GetAsync(cancellationToken: cancellationToken);

                if (photoStream == null) return null;

                using var memoryStream = new MemoryStream();
                await photoStream.CopyToAsync(memoryStream, cancellationToken);
                return $"data:image/jpeg;base64,{Convert.ToBase64String(memoryStream.ToArray())}";
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to retrieve photo for user {UserId}", userId);
                return null;
            }
        }

    }
}