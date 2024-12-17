using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witivio.Copilot4Researcher.Features.TextRewrite;
using Witivio.Copilot4Researcher.Features.TextRewrite.Models;

namespace Witivio.Copilot4Researcher.Features.TextRewrite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TextRewriteController : ControllerBase
    {
        private readonly IRewriteRulesService _rewriteRulesService;

        public TextRewriteController(IRewriteRulesService rewriteRulesService)
        {
            _rewriteRulesService = rewriteRulesService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Search for journal criteria and rewrite rules",
            Description = "This endpoint allows users to search for journal criteria and rewrite rules")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IResult> RewriteText([FromQuery] TextRewriteQuery query)
        {
            var rules = await _rewriteRulesService.GetRulesAsync(query.JournalName, query.TextType);
            if (rules == null)
            {
                return Results.BadRequest("No matching journal rules found");
            }
            return Results.Ok(rules);
        }
    }

    internal class TextAnalysisResponse
    {
        public string JournalName { get; set; }
        public int CharacterCount { get; set; }
        public int WordCount { get; set; }
        public object OriginalText { get; set; }
    }
}