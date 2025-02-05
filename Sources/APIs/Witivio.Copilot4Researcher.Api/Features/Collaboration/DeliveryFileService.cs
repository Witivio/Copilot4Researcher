using Microsoft.EntityFrameworkCore;
using Witivio.Copilot4Researcher.Core;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Search.Query;
using System.Text.Json.Serialization;
using Witivio.Copilot4Researcher.Features.Collaboration.Models;

namespace Witivio.Copilot4Researcher.Features.Collaboration
{


    public class DeliveryNoteService : IDeliveryNoteService
    {
        private readonly DeliveryNoteContext _context;
        private readonly ISharePointLogService _sharePointLogService;

        public DeliveryNoteService(DeliveryNoteContext context, ISharePointLogService sharePointLogService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _sharePointLogService = sharePointLogService ?? throw new ArgumentNullException(nameof(sharePointLogService));
        }
        public async Task<DateTime?> GetMostRecentIndexingDateAsync()
        {
            return await _context.DeliveryNotesScanFiles
                .Where(d => d.Status == IndexingStatus.Completed)
                .MaxAsync(d => (DateTime?)d.IndexingDate);
        }

        public async Task<List<Product>> SearchMostRecentProductsByKeywordsAsync(params string[] keywords)
        {
            ArgumentNullException.ThrowIfNull(keywords);
            if (!keywords.Any())
            {
                throw new ArgumentException("Keywords list cannot be empty.", nameof(keywords));
            }
            var searchCondition = string.Join(" & ", keywords);

            var products = await _context.Products
                .AsNoTracking()
                .Where(p => p.SearchVector.Matches(searchCondition))
                .Include(p => p.Delivery)
                .ThenInclude(d => d.Recipient)
                .Where(p=> p.Delivery.Date.HasValue && p.Delivery.Date.Value.ToDateTime(TimeOnly.MinValue) > DateTime.Today.AddYears(-1))
                .OrderByDescending(d => d.Delivery.Date)
                .Take(3)
                .ToListAsync();
            return products;
        }


        public async Task<DeliveryNotesScanFile> AddDeliveryNoteScanFileAsync(DeliveryNotesScanFile deliveryNotesScanFile)
        {
            ArgumentNullException.ThrowIfNull(deliveryNotesScanFile);

            var existingFile = await _context.DeliveryNotesScanFiles
                .FirstOrDefaultAsync(d => d.FullPath == deliveryNotesScanFile.FullPath);

            if (existingFile is not null)
            {
                _context.Remove(existingFile);
            }

            deliveryNotesScanFile.Status = IndexingStatus.Pending;
            await _context.DeliveryNotesScanFiles.AddAsync(deliveryNotesScanFile);
            await _context.SaveChangesAsync();

            await _sharePointLogService.LogDeliveryNotesScanFileAsync(deliveryNotesScanFile, CancellationToken.None);

            return deliveryNotesScanFile;
        }

        public async Task<DeliveryNote> AddDeliveryNoteAsync(DeliveryNote deliveryNote, int deliveryNotesScanFileId)
        {
            ArgumentNullException.ThrowIfNull(deliveryNote);

            var scanFile = await _context.DeliveryNotesScanFiles.FindAsync(deliveryNotesScanFileId)
                ?? throw new NotFoundException($"DeliveryNoteScanFile with ID {deliveryNotesScanFileId} not found");

            scanFile.DeliveryNotes.Add(deliveryNote);
            await _context.SaveChangesAsync();

            return deliveryNote;
        }

        public async Task UpdateIndexingStatusAsync(int id, IndexingStatus status)
        {
            var scanFile = await _context.DeliveryNotesScanFiles.FindAsync(id)
                ?? throw new NotFoundException($"DeliveryNoteScanFile with ID {id} not found");

            scanFile.Status = status;
            if (status == IndexingStatus.Completed)
            {
                scanFile.IndexingDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            await _sharePointLogService.LogDeliveryNotesScanFileAsync(scanFile, CancellationToken.None);
        }

        public async Task FinalizeIndexingAsync(int id, int pages)
        {
            var scanFile = await _context.DeliveryNotesScanFiles.FindAsync(id)
                ?? throw new NotFoundException($"DeliveryNoteScanFile with ID {id} not found");

            scanFile.Status = IndexingStatus.Completed;
            scanFile.IndexingDate = DateTime.UtcNow;
            scanFile.TotalPages = pages;

            await _context.SaveChangesAsync();

            await _sharePointLogService.LogDeliveryNotesScanFileAsync(scanFile, CancellationToken.None);
        }

       
    }

    public interface IDeliveryNoteService
    {
        /// <summary>
        /// Searches for a recipient based on product keywords.
        /// </summary>
        /// <param name="keywords">The search keywords.</param>
        /// <returns>The matching products or null if not found.</returns>
        Task<List<Product>> SearchMostRecentProductsByKeywordsAsync(params string[] keywords);

        /// <summary>
        /// Adds a new delivery note scan file to the system.
        /// </summary>
        Task<DeliveryNotesScanFile> AddDeliveryNoteScanFileAsync(DeliveryNotesScanFile deliveryNote);

        /// <summary>
        /// Updates the indexing status of a delivery note scan file.
        /// </summary>
        Task UpdateIndexingStatusAsync(int id, IndexingStatus status);

        /// <summary>
        /// Adds a delivery note and associates it with a scan file.
        /// </summary>
        Task<DeliveryNote> AddDeliveryNoteAsync(DeliveryNote delivery, int deliveryNoteId);

        /// <summary>
        /// Gets the most recent indexing date from completed scan files.
        /// </summary>
        Task<DateTime?> GetMostRecentIndexingDateAsync();

        /// <summary>
        /// Finalizes the indexing process for a scan file.
        /// </summary>
        Task FinalizeIndexingAsync(int id, int pages);

    }
}
