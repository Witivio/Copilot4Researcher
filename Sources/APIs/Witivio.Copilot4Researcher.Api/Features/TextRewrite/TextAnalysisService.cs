using ClosedXML.Excel;
using Microsoft.Graph;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using static Microsoft.Graph.Constants;
using System.Text.RegularExpressions;
using Microsoft.Graph.Models;
using Witivio.Copilot4Researcher.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Witivio.Copilot4Researcher.Api.Core.Options;
using Witivio.Copilot4Researcher.Features.TextRewrite.Models;
using Witivio.Copilot4Researcher.Api.Core;
namespace Witivio.Copilot4Researcher.Features.TextRewrite
{
    /// <summary>
    /// Defines operations for retrieving text analysis rules
    /// </summary>
    public interface IRewriteRulesService
    {
        Task<TextAnalysisRule> GetRulesAsync(string journalName, TextType journalType);
    }


    public class RewriteRulesSharePointService : IRewriteRulesService
    {
        private readonly GraphServiceClient _graphClient;
        private readonly SharePointOptions _options;
        private readonly ILogger<RewriteRulesSharePointService> _logger;
      

        public RewriteRulesSharePointService(
            GraphServiceClient graphClient,
            IOptions<SharePointOptions> options,
            ILogger<RewriteRulesSharePointService> logger)
        {
            _graphClient = graphClient ?? throw new ArgumentNullException(nameof(graphClient));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TextAnalysisRule> GetRulesAsync(string journalName, TextType textType)
        {
            ArgumentException.ThrowIfNullOrEmpty(journalName);

            try
            {
                var site = await GetSharePointSiteAsync();
                var listItems = await GetListItemsAsync(site.Id);

                if (listItems?.Value is null)
                    return null;

                return FindMatchingRule(listItems.Value, journalName, textType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting rules for journal {JournalName} and type {TextType}",
                    journalName, textType);
                return null;
            }
        }

        private async Task<Site> GetSharePointSiteAsync()
    {
        return await SharePointSiteHelper.GetSiteAsync(_graphClient, _options.Url);
    }

        private async Task<ListItemCollectionResponse> GetListItemsAsync(string siteId)
        {
            var listId = _options.ListId
                ?? throw new InvalidOperationException("SharePoint List ID not configured");

            return await _graphClient.Sites[siteId].Lists[listId].Items
                .GetAsync((requestConfiguration) =>
                {
                    requestConfiguration.QueryParameters.Expand = new[]
                    {
                        "fields($select=Title,MaxWords,TextType)"
                    };
                });
        }

        private TextAnalysisRule FindMatchingRule(
            IEnumerable<ListItem> listItems,
            string journalName,
            TextType textType)
        {
            var searchSoundex = Soundex.Get(journalName);

            var bestMatch = listItems
                .Select(item => new
                {
                    JournalName = GetFieldValue(item, "Title"),
                    MaxWord = GetFieldValue(item, "MaxWords"),
                    TextType = GetFieldValue(item, "TextType")
                })
                .Where(item => !string.IsNullOrEmpty(item.JournalName) &&
                              item.TextType?.Equals(textType.ToString(), StringComparison.OrdinalIgnoreCase) == true)
                .Select(item => new
                {
                    Item = item,
                    SoundexCode = Soundex.Get(item.JournalName)
                })
                .FirstOrDefault(x => x.SoundexCode == searchSoundex);

            return bestMatch == null ? null : new TextAnalysisRule
            {
                WordCount = bestMatch.Item.MaxWord,
                JournalName = bestMatch.Item.JournalName
            };
        }

        private static string GetFieldValue(ListItem item, string fieldName)
        {
            return item.Fields.AdditionalData.TryGetValue(fieldName, out var value)
                ? value?.ToString()
                : null;
        }

       
    }
}