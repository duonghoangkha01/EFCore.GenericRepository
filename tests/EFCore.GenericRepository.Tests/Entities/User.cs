using EFCore.GenericRepository.Entities;

namespace EFCore.GenericRepository.Tests.Entities
{
    public class User : EntityBase<int>
    {
        public string Name { get; set; } = string.Empty;
        public int? RoleId { get; set; }
        public Role? Role { get; set; }
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
