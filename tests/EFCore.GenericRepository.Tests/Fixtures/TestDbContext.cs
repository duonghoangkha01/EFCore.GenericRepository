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
    }
}
