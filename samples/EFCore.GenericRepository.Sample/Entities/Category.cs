using EFCore.GenericRepository.Entities;

namespace EFCore.GenericRepository.Sample.Entities
{
    public class Category : EntityBase<int>
    {
        public string Name { get; set; } = default!;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
