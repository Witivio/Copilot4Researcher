using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using Witivio.Copilot4Researcher.Core;
using Witivio.Copilot4Researcher.Features.ClinicalTrials.Cards;
using Witivio.Copilot4Researcher.Features.ClinicalTrials.Models;

namespace Witivio.Copilot4Researcher.Features.ClinicalTrials.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
    public class ClinicalTrialsController : ControllerBase
    {
        private readonly IClinicalTrialsClient _clinicalTrialsClient;

        private readonly ILogger<ClinicalTrialsController> _logger;

        public ClinicalTrialsController(IClinicalTrialsClient clinicalTrialsClient, ILogger<ClinicalTrialsController> logger)
        {
            _clinicalTrialsClient = clinicalTrialsClient;
            _logger = logger;
        }

        [HttpGet("search")]
        [SwaggerOperation(
            Summary = "Search clinical trials",
            Description = "Search for clinical trials using keywords"
        )]
        [SwaggerResponse(200, "Returns a list of clinical trials matching the search criteria")]
        public async Task<IResult> Search([FromQuery] ClinicalTrialInputSearchQuery query)
        {
            if (query == null)
                return Results.BadRequest("Query cannot be null.");

            var clinicalTrials = await _clinicalTrialsClient.SearchAsync(query.ToQuery());

            var resultTemplates = new CopilotResultTemplates();
            var renderingResults = new List<CopilotRenderingResult<ClinicalTrial>>();

            for (int i = 0; i < clinicalTrials.Count() && i < 5; i++)
            {
                var clinicalTrial = clinicalTrials.ElementAt(i);

                resultTemplates.AddCard($"card{i}", ClinicalTrialCardRenderer.Render(clinicalTrial));

                renderingResults.Add(new CopilotRenderingResult<ClinicalTrial>
                {
                    Result = clinicalTrial,
                    DisplayTemplate = $"$.templates.card{i}"
                });
            }

            var renderingResultsWrapper = new CopilotRenderingResultsWrapper<ClinicalTrial>
            {
                Templates = resultTemplates,
                Results = renderingResults
            };

            return Results.Ok(renderingResultsWrapper);
        }


    }
}
