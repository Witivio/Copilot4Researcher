using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Search.Query;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using SkiaSharp;
using System.Text.Json;
using Witivio.Copilot4Researcher.Api.Core.Options;
using Witivio.Copilot4Researcher.Core;
using Witivio.Copilot4Researcher.Features.Collaboration.Models;

namespace Witivio.Copilot4Researcher.Features.Collaboration
{


    public class DeliveryScanBackgroundService : BackgroundService
    {
        private readonly ILogger<DeliveryScanBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly PeriodicTimer _timer;
        private readonly SharePointOptions _sharePointOptions;

        public DeliveryScanBackgroundService(
            ILogger<DeliveryScanBackgroundService> logger,
            IServiceProvider serviceProvider,
            IOptions<SharePointOptions> sharepointOptions, IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _sharePointOptions = sharepointOptions.Value;
            _timer = new PeriodicTimer(TimeSpan.FromMinutes(_sharePointOptions.ScanIntervalMinutes));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (await _timer.WaitForNextTickAsync(stoppingToken))
                {
                    _logger.LogInformation("Running periodic task at: {Time}", DateTimeOffset.Now);
                    _timer.Period = Timeout.InfiniteTimeSpan; // Pause the timer
                    try
                    {
                        await DoWorkAsync(stoppingToken);
                    }
                    finally
                    {
                        _timer.Period = TimeSpan.FromMinutes(_sharePointOptions.ScanIntervalMinutes); // Resume the timer
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Periodic background service is stopping.");
            }
        }

        private async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var services = GetRequiredServices(scope);

                var mostRecentIndexingDate = await services.DeliveryService.GetMostRecentIndexingDateAsync();
                var searchResults = await SearchFilesAsync(services.GraphClient, mostRecentIndexingDate, stoppingToken);

                await ProcessSearchResultsAsync(searchResults, services, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing SharePoint files");
            }
        }

        private record RequiredServices(
            GraphServiceClient GraphClient,
            Kernel Kernel,
            IChatCompletionService ChatCompletionService,
            IDeliveryNoteService DeliveryService);

        private RequiredServices GetRequiredServices(IServiceScope scope)
        {
            return new RequiredServices(
                scope.ServiceProvider.GetRequiredService<GraphServiceClient>(),
                scope.ServiceProvider.GetRequiredService<Kernel>(),
                scope.ServiceProvider.GetRequiredService<IChatCompletionService>(),
                scope.ServiceProvider.GetRequiredService<IDeliveryNoteService>());
        }

        private async Task<IEnumerable<SearchHit>> SearchFilesAsync(
            GraphServiceClient graphClient,
            DateTime? mostRecentIndexingDate,
            CancellationToken cancellationToken)
        {
            var searchRequest = new List<SearchRequest>
            {
                new()
                {
                    EntityTypes = new List<EntityType?> { EntityType.DriveItem },
                    Region = "FRA",
                    Size = 500,
                    Query = new SearchQuery
                    {
                        QueryString = GenerateQueryString(mostRecentIndexingDate)
                    }
                }
            };

            var items = await graphClient.Search.Query
                .PostAsQueryPostResponseAsync(
                    new QueryPostRequestBody { Requests = searchRequest },
                    cancellationToken: cancellationToken);

            return items.Value
                .SelectMany(r => r.HitsContainers.FirstOrDefault()?.Hits ?? Enumerable.Empty<SearchHit>());
        }

        private async Task ProcessSearchResultsAsync(
            IEnumerable<SearchHit> searchHits,
            RequiredServices services,
            CancellationToken cancellationToken)
        {
            foreach (var hit in searchHits)
            {
                var driveItem = hit.Resource as DriveItem;
                if (driveItem == null) continue;

                var scanFile = await CreateAndSaveScanFileAsync(driveItem, services.DeliveryService);
                await ProcessPdfFileAsync(driveItem, scanFile, services, cancellationToken);
            }
        }

        private async Task<DeliveryNotesScanFile> CreateAndSaveScanFileAsync(
            DriveItem driveItem,
            IDeliveryNoteService deliveryService)
        {
            var scanFile = new DeliveryNotesScanFile
            {
                FileName = driveItem.Name,
                FullPath = driveItem.WebUrl,
                Status = IndexingStatus.Pending
            };

            await deliveryService.AddDeliveryNoteScanFileAsync(scanFile);
            _logger.LogInformation("Processing file: {FileName} at path: {FullPath}",
                scanFile.FileName,
                scanFile.FullPath);

            return scanFile;
        }

        private async Task ProcessPdfFileAsync(
            DriveItem driveItem,
            DeliveryNotesScanFile scanFile,
            RequiredServices services,
            CancellationToken cancellationToken)
        {
            var pdfContent = await DownloadPdfContentAsync(driveItem, services.GraphClient, cancellationToken);
            await services.DeliveryService.UpdateIndexingStatusAsync(scanFile.Id, IndexingStatus.Indexing);

            _logger.LogInformation("Downloaded PDF {FileName}, size: {Size} bytes",
                driveItem.Name,
                pdfContent.Length);

            var pages = await ProcessPdfPagesAsync(pdfContent, scanFile, services, cancellationToken);
            await services.DeliveryService.FinalizeIndexingAsync(scanFile.Id, pages);
        }

        private async Task<byte[]> DownloadPdfContentAsync(
            DriveItem driveItem,
            GraphServiceClient graphClient,
            CancellationToken cancellationToken)
        {
            using var stream = await graphClient.Drives[driveItem.ParentReference.DriveId]
                .Items[driveItem.Id].Content
                .GetAsync(cancellationToken: cancellationToken);

            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream, cancellationToken);
            return memoryStream.ToArray();
        }

        private async Task<int> ProcessPdfPagesAsync(
            byte[] pdfContent,
            DeliveryNotesScanFile scanFile,
            RequiredServices services,
            CancellationToken cancellationToken)
        {
            var pageCount = 0;
            var bitmaps = PDFtoImage.Conversion.ToImagesAsync(pdfContent);

            await foreach (var bitmap in bitmaps.WithCancellation(cancellationToken))
            {
                _logger.LogInformation("Processing PDF {FullPath} - page {Page}",
                    scanFile.FullPath,
                    pageCount);

                await ProcessPageImageAsync(bitmap, services, scanFile.Id, cancellationToken);
                pageCount++;
            }

            return pageCount;
        }

        private async Task ProcessPageImageAsync(
            SKBitmap bitmap,
            RequiredServices services,
            int scanFileId,
            CancellationToken cancellationToken)
        {
            using var image = SKImage.FromBitmap(bitmap);
            using var encodedData = image.Encode(SKEncodedImageFormat.Jpeg, 80);
            var encoded = encodedData.ToArray();

            var chatHistory = CreateChatHistory(encoded);
            var reply = await services.ChatCompletionService
                .GetChatMessageContentAsync(chatHistory, cancellationToken: cancellationToken);

            var options = new JsonSerializerOptions
            {
                Converters = { new NullableDateOnlyConverter() },
                PropertyNameCaseInsensitive = true // Optional: Allows case-insensitive matching of property names
            };

            var delivery = JsonSerializer.Deserialize<DeliveryNote>(SanitizeJsonContent(reply.Content));
            // Filter out products with empty names to avoid processing invalid entries returned by the AI
            delivery.Products = delivery.Products?.Where(p => !string.IsNullOrWhiteSpace(p.Name)).ToList();

            await ProcessProductsAsync(delivery, services, cancellationToken);

            await services.DeliveryService.AddDeliveryNoteAsync(delivery, scanFileId);

        }

        private async Task ProcessProductsAsync(DeliveryNote delivery, RequiredServices services, CancellationToken cancellationToken)
        {
            foreach (var product in delivery.Products)
            {
                product.Keywords = await services.Kernel.InvokePromptAsync<string>(GetCategorizeProductPrompt(product.Name), cancellationToken: cancellationToken);
            }
        }

        private static string SanitizeJsonContent(string content)
        {
            return content.Replace("```json", "").Replace("`", "").Trim();
        }

        private ChatHistory CreateChatHistory(byte[] imageData)
        {
            const string systemPrompt = "You are a friendly assistant that helps extract data from images.";
            var chatHistory = new ChatHistory(systemPrompt);

            chatHistory.AddUserMessage(new ChatMessageContentItemCollection
            {
                new TextContent(GetExtractProductPrompt()),
                new ImageContent(new ReadOnlyMemory<byte>(imageData), "image/jpg")
            });

            return chatHistory;
        }

        private string GetExtractProductPrompt()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "features/collaboration/prompts/ExtractProductPrompt.txt");
            return File.ReadAllText(path);
        }

        private string GetCategorizeProductPrompt(string productName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(productName);

            var path = Path.Combine(Environment.CurrentDirectory, "features/collaboration/prompts/CategorizeProductPrompt.txt");

            return productName + Environment.NewLine + File.ReadAllText(path);
        }

        private string GenerateQueryString(DateTime? mostRecentIndexingDate)
        {
            var sharePointUrl = _sharePointOptions.DeliveryNotesUrl;
            if (string.IsNullOrEmpty(sharePointUrl))
            {
                throw new InvalidOperationException("SharePoint URL is not configured.");
            }

            var baseQuery = $"path:\"{sharePointUrl}\" AND FileType:pdf";

            if (mostRecentIndexingDate.HasValue)
            {
                return $"{baseQuery} AND LastModifiedTime > {mostRecentIndexingDate.Value:yyyy-MM-ddTHH:mm:ssZ}";
            }

            return baseQuery;
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Periodic background service is stopping.");
            await base.StopAsync(stoppingToken);
        }
    }
}