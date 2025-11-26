using EFCore.GenericRepository.Repositories;
using EFCore.GenericRepository.Tests.Entities;
using EFCore.GenericRepository.Tests.Fixtures;

namespace EFCore.GenericRepository.Tests.Repositories
{
    /// <summary>
    /// Test repository for Post entity (regular entity without soft delete).
    /// </summary>
    public class PostRepository : RepositoryBase<Post, int, TestDbContext>
    {
        public PostRepository(TestDbContext dbContext) : base(dbContext)
        {
        }
    }
}
