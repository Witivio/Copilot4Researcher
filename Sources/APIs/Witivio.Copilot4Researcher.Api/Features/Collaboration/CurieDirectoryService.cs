using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System.Web;
using Witivio.Copilot4Researcher.Api.Features.Collaboration.Models;

namespace Witivio.Copilot4Researcher.Api.Features.Collaboration;

public interface ICurieDirectoryService
{
    Task<List<CuriePersonResult>> SearchByNameAsync(string lastName, CancellationToken cancellationToken = default);
}

public class CurieDirectoryService : ICurieDirectoryService
{
    private const string BaseUrl = "https://en.backend.curie.fr/graphql";
    private readonly HttpClient _httpClient;
    private readonly ILogger<CurieDirectoryService> _logger;

    public CurieDirectoryService(HttpClient httpClient, ILogger<CurieDirectoryService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));


    }

    public async Task<List<CuriePersonResult>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        // Split the name by spaces, remove items that contain a period, and keep parts with more than 2 letters
        var nameParts = name.Split(' ')
                            .Where(part => !part.Contains('.') && part.Length > 2)
                            .ToArray();
        var filteredName = string.Join(" ", nameParts);

        try
        {
            var graphQlRequest = new
            {
                operationName = "ListPersonSearchQuery",
                variables = new
                {
                    langcode = "en",
                    query = filteredName,
                    filters = new[]
                    {
                        new { conditions = new[] { new { field = "status", values = "1" } }, conjunction = "or" },
                        new { conditions = new[] { new { field = "type", values = "person" } }, conjunction = "or" },
                        new { conditions = new[] { new { field = "field_person_site", values = "171" } }, conjunction = "or" }
                    },
                    limit = 1,
                    offset = 0,
                    sort = new { field = "search_api_relevance", order = "DESC" }
                },
                query = @"query ListPersonSearchQuery($langcode: String!, $query: String, $filters: [InputOpenSearchConditionsGroup], $limit: Int, $offset: Int, $sort: InputSort) {
                    opensearchSearch(index: ""curie"", limit: $limit, langcode: $langcode, filters: $filters, query: $query, offset: $offset, sort: $sort) {
                        documents {
                            entity {
                                ... on NodePerson {
                                    fieldFirstName
                                    fieldLastName
                                    fieldImage {
                                        entity {
                                            uriRawField {
                                                list {
                                                    url
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }"
            };

            var response = await _httpClient.PostAsJsonAsync(BaseUrl, graphQlRequest, cancellationToken);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<GraphQLResponse>(cancellationToken);
            return ExtractPeopleFromGraphQL(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching Curie directory for name: {Name}", name);
            return new List<CuriePersonResult>();
        }
    }

    private static List<CuriePersonResult> ExtractPeopleFromGraphQL(GraphQLResponse response)
    {
        if (response?.Data?.OpensearchSearch?.Documents == null)
            return new List<CuriePersonResult>();

        return response.Data.OpensearchSearch.Documents
            .Select(doc => new CuriePersonResult
            {
                FirstName = doc.Entity.FieldFirstName,
                LastName = doc.Entity.FieldLastName,
                PhotoUrl = doc.Entity.FieldImage?.Entity?.UriRawField?.List?.FirstOrDefault()?.Url
            })
            .ToList();
    }


    // Add these classes to handle GraphQL response
    public class GraphQLResponse
    {
        public GraphQLData Data { get; set; }
    }

    public class GraphQLData
    {
        public OpensearchSearch OpensearchSearch { get; set; }
    }

    public class OpensearchSearch
    {
        public List<Document> Documents { get; set; }
    }

    public class Document
    {
        public Entity Entity { get; set; }
    }

    public class Entity
    {
        public string FieldFirstName { get; set; }
        public string FieldLastName { get; set; }
        public FieldImage FieldImage { get; set; }
    }

    public class FieldImage
    {
        public ImageEntity Entity { get; set; }
    }

    public class ImageEntity
    {
        public UriRawField UriRawField { get; set; }
    }

    public class UriRawField
    {
        public List<UrlItem> List { get; set; }
    }

    public class UrlItem
    {
        public string Url { get; set; }
    }

}
