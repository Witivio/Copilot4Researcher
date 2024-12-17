using Microsoft.Graph;
using Microsoft.Graph.Models;
using System;
using System.Text.RegularExpressions;
namespace Witivio.Copilot4Researcher.Api.Core;
public class SharePointSiteHelper
{
    private const string SharePointUrlPattern = @"^(?<scheme>https:\/\/){1}(?<domain>[A-Za-z0-9]{1,})(.sharepoint.com){1}(?<sites>\/sites)?(?<siteName>\/[A-Za-z0-9-%.]*)(\/[\S]*)?$";
    private static readonly TimeSpan RegexTimeout = TimeSpan.FromSeconds(1);
    private static readonly Regex SharePointUrlRegex = new(SharePointUrlPattern, RegexOptions.Compiled, RegexTimeout);

    public static async Task<Site> GetSiteAsync(GraphServiceClient graphClient, string url)
    {
        if (string.IsNullOrEmpty(url))
            throw new ArgumentNullException(nameof(url), "SharePoint URL not configured");

        var fullUrl = ParseSharePointUrl(url);
        return await graphClient.Sites[fullUrl].GetAsync()
            ?? throw new InvalidOperationException("SharePoint site not found");
    }

    private static string ParseSharePointUrl(string url)
    {
        var match = SharePointUrlRegex.Match(url);
        if (!match.Success)
        {
            throw new ArgumentException("Invalid SharePoint URL format", nameof(url));
        }

        var siteName = match.Groups["siteName"].Value;
        var domain = match.Groups["domain"].Value;
        var hostName = $"{domain}.sharepoint.com";
        var relativePath = url.Contains("/sites/")
            ? $"sites{siteName}"
            : siteName;

        return $"{hostName}:/{relativePath}";
    }
} 