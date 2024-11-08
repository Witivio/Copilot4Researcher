using AdaptiveCards;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using System.Text.Json;
using Witivio.Copilot4Researcher.Core;
using Witivio.Copilot4Researcher.Models;
using Witivio.Copilot4Researcher.Providers.Patents;
using Witivio.Copilot4Researcher.Providers.Patents.Cards;
using Witivio.Copilot4Researcher.Providers.ProteinAtlas;
using Witivio.Copilot4Researcher.Providers.ProteinAtlas.Cards;

namespace Witivio.Copilot4Researcher.Controllers
{



    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
    public class PatentController : ControllerBase
    {


        private readonly ILogger<PatentController> _logger;
        private readonly IPatentsClient patentsClient;


        public PatentController(ILogger<PatentController> logger, IPatentsClient patentsClient)
        {
            _logger = logger;
            this.patentsClient = patentsClient;


        }

        [HttpGet]
        [Route("Search")]
        [SwaggerOperation(
            Summary = "Search for patent",
            Description = "This endpoint allows users to search patent")]
        [ProducesResponseType(typeof(CopilotRenderingResultsWrapper<Patent>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchAsync([FromQuery] PatentSearchInputQuery query)
        {
            _logger.LogInformation("Patent search requested with query: {@Query}", query);

            if (query == null)
            {
                _logger.LogWarning("Search attempt with null query");
                return BadRequest("Query cannot be null.");
            }

            try
            {
                var searchQuery = query.ToQuery();
                _logger.LogInformation("Executing search with query: {Query}", query);

                var patents = await patentsClient.SearchAsync(searchQuery);

                var (templates, rendering) = CreatePatentResults(patents.Take(5));

                var response = new CopilotRenderingResultsWrapper<Patent>
                {
                    Templates = templates,
                    Results = rendering
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching patents with query: {@Query}", query);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        private static (CopilotResultTemplates templates, List<CopilotRenderingResult<Patent>> rendering) CreatePatentResults(IEnumerable<Patent> patents)
        {
            var templates = new CopilotResultTemplates();
            var rendering = new List<CopilotRenderingResult<Patent>>();

            var index = 0;
            foreach (var patent in patents)
            {
                var cardId = $"card{index}";
                templates.AddCard(cardId, PatentCardRenderer.Render(patent));

                rendering.Add(new CopilotRenderingResult<Patent>
                {
                    Result = patent,
                    DisplayTemplate = $"$.templates.{cardId}"
                });

                index++;
            }

            return (templates, rendering);
        }
    }


}
