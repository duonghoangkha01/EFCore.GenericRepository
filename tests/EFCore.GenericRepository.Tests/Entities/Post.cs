using EFCore.GenericRepository.Entities;

namespace EFCore.GenericRepository.Tests.Entities
{
    /// <summary>
    /// Test entity for navigation property testing.
    /// </summary>
    public class Post : EntityBase<int>
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
