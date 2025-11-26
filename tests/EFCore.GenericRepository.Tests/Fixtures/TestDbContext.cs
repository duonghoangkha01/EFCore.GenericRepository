using EFCore.GenericRepository.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCore.GenericRepository.Tests.Fixtures
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<SoftDeletableProduct> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User-Role relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);

            // Configure Post-User relationship
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId);

            // Configure soft delete global query filter for SoftDeletableProduct
            modelBuilder.Entity<SoftDeletableProduct>()
                .HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
