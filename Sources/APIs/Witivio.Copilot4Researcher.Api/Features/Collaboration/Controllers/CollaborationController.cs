using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using Witivio.Copilot4Researcher.Api.Features.Collaboration;
using Witivio.Copilot4Researcher.Core;
using Witivio.Copilot4Researcher.Features.Collaboration;
using Witivio.Copilot4Researcher.Features.Collaboration.Cards;
using Witivio.Copilot4Researcher.Features.Collaboration.Models;
using Recipient = Witivio.Copilot4Researcher.Features.Collaboration.Models.Recipient;

namespace Witivio.Copilot4Researcher.Features.Collaboration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CollaborationController : ControllerBase
    {
        private readonly IDeliveryNoteService _deliveryNoteService;
        private readonly IUserService _userService;

        public CollaborationController(IDeliveryNoteService deliveryNoteService, IUserService userService)
        {
            _deliveryNoteService = deliveryNoteService;
            _userService = userService;
        }

        [HttpGet("search")]
        [SwaggerOperation(
            Summary = "Search for people",
            Description = "This endpoint allows users to search people who already use a product or already did an experiment")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IResult> SearchAsync([FromQuery][Required] CollaborationInputQuery query)
        {
            if (query == null)
                return Results.BadRequest("Query cannot be null.");

            var products = await _deliveryNoteService.SearchMostRecentProductsByKeywordsAsync(query.ToQuery().Keywords);
            var recipientProduct = products.Select(p => new { Product = p, p.Delivery.Recipient }).ToList();

            var resultTemplates = new CopilotResultTemplates();
            var renderingResults = new List<CopilotRenderingResult<object>>();

            for (int i = 0; i < recipientProduct.Count() & i < 3; i++)
            {
                var recipient = recipientProduct.ElementAt(i);

                var user = await _userService.SearchAsync(recipient.Recipient.Name);
                if (user != null)
                {
                    resultTemplates.AddCard($"card{i}", ContactCardRenderer.Render(user, recipient.Product));

                    renderingResults.Add(new CopilotRenderingResult<object>
                    {
                        Result = user,
                        DisplayTemplate = $"$.templates.card{i}"
                    });
                }
                else
                {
                    resultTemplates.AddCard($"card{i}", FallbackContactCardRenderer.Render(recipient.Recipient, recipient.Product));

                    renderingResults.Add(new CopilotRenderingResult<object>
                    {
                        Result = new { recipient.Recipient.Name, recipient.Recipient.UnitName },
                        DisplayTemplate = $"$.templates.card{i}"
                    });
                }
            }

            var renderingResultsWrapper = new CopilotRenderingResultsWrapper<object>
            {
                Templates = resultTemplates,
                Results = renderingResults
            };

            return Results.Ok(renderingResultsWrapper);
        }
    }
}
