using EFCore.GenericRepository.Repositories;
using EFCore.GenericRepository.Tests.Entities;
using EFCore.GenericRepository.Tests.Fixtures;

namespace EFCore.GenericRepository.Tests.Repositories
{
    public class TestRepository : RepositoryQueryBase<User, int, TestDbContext>
    {
        public TestRepository(TestDbContext dbContext) : base(dbContext)
        {
        }
    }
}
