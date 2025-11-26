using EFCore.GenericRepository.Entities;

namespace EFCore.GenericRepository.Tests.Entities
{
    /// <summary>
    /// Test entity that implements ISoftDeletable for soft delete testing.
    /// </summary>
    public class SoftDeletableProduct : EntityBase<int>, ISoftDeletable
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
