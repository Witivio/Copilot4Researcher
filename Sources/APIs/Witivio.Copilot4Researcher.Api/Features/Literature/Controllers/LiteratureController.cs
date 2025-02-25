using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using System.Text;
using Witivio.Copilot4Researcher.Core;
using Witivio.Copilot4Researcher.Features.Literature.BioRxiv;
using Witivio.Copilot4Researcher.Features.Literature.BioRxiv.Cards;
using Witivio.Copilot4Researcher.Features.Literature.HAL;
using Witivio.Copilot4Researcher.Features.Literature.HAL.Cards;
using Witivio.Copilot4Researcher.Features.Literature.Scimago;
using Witivio.Copilot4Researcher.Features.Literature.Pubmed;
using Witivio.Copilot4Researcher.Features.Literature.Pubmed.Cards;
using Witivio.Copilot4Researcher.Features.Literature.Models;

namespace Witivio.Copilot4Researcher.Features.Literature.Controllers
{

    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
    public class LiteratureController : ControllerBase
    {
        private readonly ILogger<LiteratureController> _logger;
        private readonly IPubmedClient _pubmedClient;
        private readonly IBioRxivClient _bioRxivClient;
        private readonly IHALClient _halClient;
        private readonly IJournalDataService _journalDataService;

        public LiteratureController(ILogger<LiteratureController> logger,
            IPubmedClient pubmedClient,
            IBioRxivClient bioRxivClient,
            IHALClient halClient,
            IJournalDataService journalDataService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pubmedClient = pubmedClient ?? throw new ArgumentNullException(nameof(pubmedClient));
            _bioRxivClient = bioRxivClient ?? throw new ArgumentNullException(nameof(bioRxivClient));
            _halClient = halClient ?? throw new ArgumentNullException(nameof(halClient));
            _journalDataService = journalDataService ?? throw new ArgumentNullException(nameof(journalDataService));
        }

        [HttpGet("Search")]
        [SwaggerOperation(
            Summary = "Search for life science literature",
            Description = "This endpoint allows users to search for life science research articles using either natural language inputs or specific parameters like keywords, date ranges, and authors. The query interprets user intent, applying filters to return relevant, high-quality articles from databases like PubMed and Google Scholar. Users can specify their preferences, such as 'Show latest articles about BRCA1,' and the system will automatically retrieve the most relevant results.")]
        [ProducesResponseType(typeof(IEnumerable<Publication>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IResult> SearchAsync([FromQuery] LiteratureSearchInputQuery inputQuery)
        {
            try
            {
                if (inputQuery == null)
                {
                    _logger.LogWarning("Search attempted with null query");
                    return Results.BadRequest("Query cannot be null.");
                }

                var query = inputQuery.ToQuery();
                _logger.LogInformation("Executing search with query: {Query}", query);

                var searchTasks = new[]
                {
                    _pubmedClient.SearchAsync(query),
                    _bioRxivClient.SearchAsync(query),
                    _halClient.SearchAsync(query)
                };

                var results = await Task.WhenAll(searchTasks);

                var publications = results
                    .SelectMany(r => r)
                    .DistinctBy(d => d.DOI)
                    .OrderByDescending(d => d.Date)
                    .ToList();

                await EnrichPublicationsWithJournalInfoAsync(publications);

                var resultTemplates = new CopilotResultTemplates();
                var renderingResults = new List<CopilotRenderingResult<Publication>>();

                foreach (var (publication, index) in publications.Select((p, i) => (p, i)))
                {
                    var cardRenderer = publication.Source switch
                    {
                        Publication.PublicationSource.Pubmed => PubmedCardRenderer.Render(publication),
                        Publication.PublicationSource.BioRxiv => BioRxivCardRenderer.Render(publication),
                        Publication.PublicationSource.HAL => HALCardRenderer.Render(publication),
                        _ => throw new ArgumentException($"Unknown publication source: {publication.Source}")
                    };

                    var cardId = $"card{index}";
                    resultTemplates.AddCard(cardId, cardRenderer);

                    renderingResults.Add(new CopilotRenderingResult<Publication>
                    {
                        Result = publication,
                        DisplayTemplate = $"$.templates.{cardId}"
                    });
                }

                var renderingResultsWrapper = new CopilotRenderingResultsWrapper<Publication>
                {
                    Templates = resultTemplates,
                    Results = renderingResults
                };

                return Results.Ok(renderingResultsWrapper);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing literature search");
                return Results.StatusCode(500);
            }
        }

        private async Task EnrichPublicationsWithJournalInfoAsync(IEnumerable<Publication> publications)
        {
            foreach (var publication in publications)
            {
                try
                {
                    var journalInfo = await _journalDataService.SearchByTitleAsync(publication.JournalName);
                    if (journalInfo?.CitesPerDoc2Years != null)
                    {
                        publication.ImpactFactor = (double)journalInfo.CitesPerDoc2Years;
                    }
                    else
                    {
                        publication.ImpactFactor = 0;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to fetch journal info for {JournalName}", publication.JournalName);
                }
            }
        }

        [HttpGet("Download/{pubmedId}/nbib")]
        [SwaggerOperation(
            Summary = "Download NBIB file for a PubMed article",
            Description = "Downloads citation information in NBIB format for a specific PubMed article using its PubMed ID.")]
        [Produces(MediaTypeNames.Application.Octet)]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IResult> DownloadNbibAsync(string pubmedId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pubmedId))
                {
                    return Results.BadRequest("PubMed ID cannot be empty.");
                }

                var publication = await _pubmedClient.GetByIdAsync(pubmedId);

                if (publication == null)
                {
                    return Results.NotFound($"No publication found for PubMed ID: {pubmedId}");
                }

                var nbibContent = CitationFormatGenerator.GenerateNbibContent(publication);
                var bytes = Encoding.UTF8.GetBytes(nbibContent);
                return Results.File(bytes, "application/x-nbib", $"pubmed_{pubmedId}.nbib");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while downloading NBIB for PubMed ID: {PubmedId}", pubmedId);
                return Results.StatusCode(500);
            }
        }

        [HttpGet("Download/{pubmedId}/bibtex")]
        [SwaggerOperation(
            Summary = "Download BibTeX file for a PubMed article",
            Description = "Downloads citation information in BibTeX format for a specific PubMed article using its PubMed ID.")]
        [Produces(MediaTypeNames.Application.Octet)]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IResult> DownloadBibtexAsync(string pubmedId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pubmedId))
                {
                    return Results.BadRequest("PubMed ID cannot be empty.");
                }

                var publication = await _pubmedClient.GetByIdAsync(pubmedId);

                if (publication == null)
                {
                    return Results.NotFound($"No publication found for PubMed ID: {pubmedId}");
                }

                var bibtexContent = CitationFormatGenerator.GenerateBibtexContent(publication);
                var bytes = Encoding.UTF8.GetBytes(bibtexContent);
                return Results.File(bytes, "application/x-bibtex", $"pubmed_{pubmedId}.bib");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while downloading BibTeX for PubMed ID: {PubmedId}", pubmedId);
                return Results.StatusCode(500);
            }
        }
    }
}
