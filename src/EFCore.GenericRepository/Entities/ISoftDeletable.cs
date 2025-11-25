namespace EFCore.GenericRepository.Entities;

/// <summary>
/// Marker interface for entities that support soft delete.
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// Gets or sets a value indicating whether this entity is soft deleted.
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the date and time when this entity was soft deleted.
    /// </summary>
    DateTimeOffset? DeletedAt { get; set; }
}
