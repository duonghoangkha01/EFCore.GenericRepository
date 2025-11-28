using EFCore.GenericRepository.Entities;

namespace EFCore.GenericRepository.Sample.Entities
{
    public class Product : EntityBase<int>, ISoftDeletable
    {
        public string Name { get; set; } = default!;
        
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; } = default!;

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }
    }
}
