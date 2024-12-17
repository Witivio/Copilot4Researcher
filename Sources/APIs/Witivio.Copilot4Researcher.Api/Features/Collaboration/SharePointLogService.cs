using Microsoft.Graph;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Witivio.Copilot4Researcher.Api.Core.Options;
using Witivio.Copilot4Researcher.Features.Collaboration.Models;
using Microsoft.Graph.Models;
using System.Text.RegularExpressions;
using Witivio.Copilot4Researcher.Api.Core;


namespace Witivio.Copilot4Researcher.Features.Collaboration;

public interface ISharePointLogService
{
    Task LogDeliveryNotesScanFileAsync(DeliveryNotesScanFile scanFile, CancellationToken cancellationToken);
}

public class SharePointLogService : ISharePointLogService
{
    private readonly GraphServiceClient _graphClient;
    private readonly ILogger<SharePointLogService> _logger;
    private readonly SharePointOptions _options;

    public SharePointLogService(
        GraphServiceClient graphClient,
        IOptions<SharePointOptions> options,
        ILogger<SharePointLogService> logger)
    {
        _graphClient = graphClient;
        _logger = logger;
        _options = options.Value;

        if (string.IsNullOrEmpty(_options.Url))
            throw new ArgumentNullException(nameof(_options.Url), "SharePoint URL is not configured");
        if (string.IsNullOrEmpty(_options.LogListId))
            throw new ArgumentNullException(nameof(_options.ListId), "SharePoint ListId is not configured");
    }

    public async Task LogDeliveryNotesScanFileAsync(DeliveryNotesScanFile scanFile, CancellationToken cancellationToken)
    {
        try
        {
            // Check if entry already exists
            var existingItem = await FindExistingListItemAsync(scanFile.FileName, cancellationToken);

            if (existingItem != null)
            {
                await UpdateListItemAsync(existingItem.Id, scanFile, cancellationToken);
                _logger.LogInformation("Updated SharePoint log entry for file: {FileName}", scanFile.FileName);
            }
            else
            {
                await CreateListItemAsync(scanFile, cancellationToken);
                _logger.LogInformation("Created new SharePoint log entry for file: {FileName}", scanFile.FileName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging delivery note scan file to SharePoint: {FileName}", scanFile.FileName);
            throw;
        }
    }
    private async Task<Site> GetSharePointSiteAsync()
    {
        return await SharePointSiteHelper.GetSiteAsync(_graphClient, _options.Url);
    }

    private async Task<ListItem> FindExistingListItemAsync(string fileName, CancellationToken cancellationToken)
    {
        var site = await GetSharePointSiteAsync();
        var items = await _graphClient.Sites[site.Id].Lists[_options.LogListId].Items
            .GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Filter = $"fields/Title eq '{fileName}'";
                requestConfiguration.QueryParameters.Select = new[] { "id" };
            }, cancellationToken);

        return items?.Value?.FirstOrDefault();
    }

    private async Task UpdateListItemAsync(string itemId, DeliveryNotesScanFile scanFile, CancellationToken cancellationToken)
    {
        var fields = CreateFieldValueDictionary(scanFile);
        var site = await GetSharePointSiteAsync();
        await _graphClient.Sites[site.Id].Lists[_options.LogListId].Items[itemId].Fields
            .PatchAsync(new FieldValueSet { AdditionalData = fields }, cancellationToken: cancellationToken);
    }

    private async Task CreateListItemAsync(DeliveryNotesScanFile scanFile, CancellationToken cancellationToken)
    {
        var fields = CreateFieldValueDictionary(scanFile);
        var site = await GetSharePointSiteAsync();
        await _graphClient.Sites[site.Id].Lists[_options.LogListId].Items
            .PostAsync(new ListItem { Fields = new FieldValueSet { AdditionalData = fields } }, cancellationToken: cancellationToken);
    }

    private static Dictionary<string, object> CreateFieldValueDictionary(DeliveryNotesScanFile scanFile)
    {
        return new Dictionary<string, object>
        {
            { "Title", scanFile.FileName },
            { "TotalPages", scanFile.TotalPages?.ToString() ?? "0" },
            { "Status", scanFile.Status.ToString() },
            { "FullPath", scanFile.FullPath },
            { "IndexingDate", scanFile.IndexingDate.ToString("yyyy-MM-dd hh:mm:ss") }
        };
    }
}
