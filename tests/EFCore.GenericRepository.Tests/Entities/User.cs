using EFCore.GenericRepository.Entities;

namespace EFCore.GenericRepository.Tests.Entities
{
    public class User : EntityBase<int>
    {
        public string Name { get; set; } = string.Empty;
    }
}
