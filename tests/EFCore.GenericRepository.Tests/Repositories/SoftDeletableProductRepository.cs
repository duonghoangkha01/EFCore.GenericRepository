using EFCore.GenericRepository.Repositories;
using EFCore.GenericRepository.Tests.Entities;
using EFCore.GenericRepository.Tests.Fixtures;

namespace EFCore.GenericRepository.Tests.Repositories
{
    /// <summary>
    /// Test repository for SoftDeletableProduct entity with full CRUD operations.
    /// </summary>
    public class SoftDeletableProductRepository : RepositoryBase<SoftDeletableProduct, int, TestDbContext>
    {
        public SoftDeletableProductRepository(TestDbContext dbContext) : base(dbContext)
        {
        }
    }
}
