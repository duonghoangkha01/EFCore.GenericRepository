using EFCore.GenericRepository.Repositories;
using EFCore.GenericRepository.Tests.Entities;
using EFCore.GenericRepository.Tests.Fixtures;

namespace EFCore.GenericRepository.Tests.Repositories
{
    /// <summary>
    /// Test repository for SoftDeletableProduct entity.
    /// </summary>
    public class SoftDeletableProductRepository : RepositoryQueryBase<SoftDeletableProduct, int, TestDbContext>
    {
        public SoftDeletableProductRepository(TestDbContext dbContext) : base(dbContext)
        {
        }
    }
}
