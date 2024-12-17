using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Witivio.Copilot4Researcher.Features.Literature.Pubmed;
using Witivio.Copilot4Researcher.Features.Literature.Controllers;
using Witivio.Copilot4Researcher.Features.Literature.Models;


namespace Witivio.Copilot4Researcher.Tests
{
    public class LiteratureControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IPubmedClient> _mockPubmedClient;
        private readonly Mock<ILogger<LiteratureController>> _mockLogger;

        public LiteratureControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _mockPubmedClient = new Mock<IPubmedClient>();
            _mockLogger = new Mock<ILogger<LiteratureController>>();
        }

        [Fact]
        public async Task SearchAsync_ReturnsOk_WithValidQuery()
        {
            // Arrange
            var query = new LiteratureSearchQuery
            {
                // Set properties of the query object as needed for the test
                Keywords = new string[] {"nadege poncet"},
                UserInput = "string",
                UserIntent = "string"
            };

            var expectedResult = new List<Publication>()
            {
                // Set expected result object
            };

            

            _mockPubmedClient.Setup(client => client.SearchAsync(It.IsAny<LiteratureSearchQuery>()))
                .ReturnsAsync(expectedResult);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Replace real PubmedClient with mocked one in DI container
                    services.AddSingleton(_mockPubmedClient.Object);
                });
            }).CreateClient();

            var content = new StringContent(JsonSerializer.Serialize(query), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/Literature", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualResult = JsonSerializer.Deserialize<IEnumerable<Publication>>(responseString);

            Assert.Equal(JsonSerializer.Serialize(expectedResult), responseString);
        }

        [Fact]
        public async Task SearchAsync_ReturnsBadRequest_WhenQueryIsNull()
        {
            // Arrange
            var client = _factory.CreateClient();
            var content = new StringContent("", Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/Literature", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
