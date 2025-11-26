using EFCore.GenericRepository.Entities;

namespace EFCore.GenericRepository.Tests.Entities
{
    public class Role : EntityBase<int>
    {
        public string Name { get; set; } = string.Empty;
    }
}
