using Microsoft.EntityFrameworkCore;
using Witivio.Copilot4Researcher.Features.Collaboration.Models;

namespace Witivio.Copilot4Researcher.Features.Collaboration
{
    public class DeliveryNoteContext : DbContext
    {
        public DeliveryNoteContext(DbContextOptions<DeliveryNoteContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<DeliveryNote> DeliveryNotes { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<DeliveryNotesScanFile> DeliveryNotesScanFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Delivery)
                .WithMany(d => d.Products)
                .HasForeignKey(p => p.DeliveryId);

            // Configure full text search for Product name
            modelBuilder.Entity<Product>()
                .HasGeneratedTsVectorColumn(
                    p => p.SearchVector,
                    "english",  // Text search configuration
                    p => new { p.Name, p.Keywords }  // Properties to include
                )
                .HasIndex(p => p.SearchVector)
                .HasMethod("GIN"); // GIN index for full text search


            // Recipient -> DeliveryNote relationship (One-to-One)
            modelBuilder.Entity<Recipient>()
                .HasOne(r => r.Delivery)
                .WithOne(d => d.Recipient)
                .HasForeignKey<Recipient>(r => r.DeliveryId);

            // DeliveryNote -> DeliveryNotesScanFile relationship (Many-to-One)
            modelBuilder.Entity<DeliveryNote>()
                .HasOne(d => d.DeliveryNotesScanFile)
                .WithMany(s => s.DeliveryNotes)
                .HasForeignKey(d => d.DeliveryNotesScanFileId);

            // Configure IndexingStatus as string in the database
            modelBuilder.Entity<DeliveryNotesScanFile>()
                .Property(e => e.Status)
                .HasConversion<string>();

        }
    }

}
