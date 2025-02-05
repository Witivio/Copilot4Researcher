using System;
using System.Net.Http;
using System.Threading.Tasks;
using Witivio.Copilot4Researcher.Features.Literature.Pubmed;
using Witivio.Copilot4Researcher.Models;
using Xunit;

namespace Witivio.Copilot4Researcher.Tests
{
    public class PubmedClientIntegrationTests
    {
        private readonly PubmedClient _pubmedClient;

        public PubmedClientIntegrationTests()
        {
            // Using the real HttpClient
            var httpClient = new HttpClient();
            _pubmedClient = new PubmedClient(httpClient);
        }

        [Fact]
        public async Task SearchAsync_ReturnsResults_WhenValidQuery()
        {
            // Arrange
            var query = new LiteratureSearchQuery
            {
                Keywords = new [] { "poncet" },  // Example keyword
            };

            // Act
            var result = await _pubmedClient.SearchAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);  // Ensure some results are returned
        }

        [Fact]
        public async Task SearchAsync_ReturnsEmpty_WhenNoMatches()
        {
            // Arrange
            var query = new LiteratureSearchQuery
            {
                Keywords = new[] { "nonexistentkeyword" },
                Authors = new[] { "NoSuchAuthor" }
            };

            // Act
            var result = await _pubmedClient.SearchAsync(query);

            // Assert
            Assert.Empty(result);  // Expect no results for this query
        }
    }
}