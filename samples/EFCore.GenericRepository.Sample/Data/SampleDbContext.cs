using EFCore.GenericRepository.Extensions;
using EFCore.GenericRepository.Sample.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCore.GenericRepository.Sample.Data
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationship between Category and Product
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            // IMPORTANT: This extension method scans the assembly for entities that implement ISoftDeletable
            // and applies a global query filter to them so they will be excluded from queries by default.
            modelBuilder.ConfigureSoftDelete();
        }
    }
}
