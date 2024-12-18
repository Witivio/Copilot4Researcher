using AdaptiveCards;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Text.Json;
using Witivio.Copilot4Researcher.Core;
using Witivio.Copilot4Researcher.Features.Gene.Models;
using Witivio.Copilot4Researcher.Features.Gene.ProteinAtlas;
using Witivio.Copilot4Researcher.Features.Gene.ProteinAtlas.Cards;

namespace Witivio.Copilot4Researcher.Features.Gene.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
    public class GeneController : ControllerBase
    {
        private readonly ILogger<GeneController> _logger;
        private readonly IProteinAtlasClient _proteinAtlasClient;


        public GeneController(ILogger<GeneController> logger, IProteinAtlasClient proteinAtlasClient)
        {
            _logger = logger;
            _proteinAtlasClient = proteinAtlasClient;

        }

        [HttpGet]
        [Route("Search")]
        [SwaggerOperation(
            Summary = "Search for protein expression",
            Description = "This endpoint allows users to search expression and detail of a protein.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IResult> SearchAsync([FromQuery][Required] GeneSearchQuery query)
        {
            if (query == null)
                return Results.BadRequest("Query cannot be null.");

            _logger.LogInformation("Searching for gene: {GeneName}", query.Name);

            var geneResult = await _proteinAtlasClient.SearchAsync(query);

            if (geneResult == null)
                return Results.NotFound($"The gene {query.Name} can not be found.");

            var cardTemplates = new CopilotResultTemplates();
            cardTemplates.AddCard("card", GeneCardRenderer.Render(geneResult));

            var resultWrapper = new CopilotRenderingResultWrapper<Models.Gene>();
            resultWrapper.Templates = cardTemplates;
            resultWrapper.Result = geneResult;
            resultWrapper.DisplayTemplate = "$.templates.card";

            return Results.Ok(resultWrapper);
        }
    }


}
