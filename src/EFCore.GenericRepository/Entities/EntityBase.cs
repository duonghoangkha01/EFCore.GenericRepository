namespace EFCore.GenericRepository.Entities;

/// <summary>
/// Base class for all entities with generic primary key type.
/// </summary>
/// <typeparam name="K">The type of the primary key.</typeparam>
public abstract class EntityBase<K>
{
    /// <summary>
    /// Gets or sets the primary key.
    /// </summary>
    public K Id { get; set; } = default!;
}
