using EFCore.GenericRepository.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCore.GenericRepository.Repositories.Interfaces
{
    /// <summary>
    /// Unified repository interface combining CRUD operations and fluent query building capabilities.
    /// This interface provides the complete set of repository operations including both
    /// context-independent commands and EF Core-specific query building methods.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="K">The entity's primary key type.</typeparam>
    /// <typeparam name="TContext">The DbContext type.</typeparam>
    /// <remarks>
    /// <para>
    /// This interface combines:
    /// </para>
    /// <list type="bullet">
    /// <item><description><see cref="IRepositoryBase{T, K}"/> - All CRUD operations and simple queries</description></item>
    /// <item><description><see cref="IRepositoryQueryBase{T, K, TContext}"/> - EF Core-specific fluent query building (Include, OrderBy, AsNoTracking)</description></item>
    /// </list>
    /// <para>
    /// Use this interface when you need both command operations (Add, Update, Delete) and
    /// complex query building in the same service. For better separation of concerns:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Use <see cref="IRepositoryBase{T, K}"/> for services that only need CRUD operations</description></item>
    /// <item><description>Use <see cref="IRepositoryQueryBase{T, K, TContext}"/> for services that only need complex queries</description></item>
    /// </list>
    /// </remarks>
    public interface IRepositoryBase<T, K, TContext>
        : IRepositoryBase<T, K>,
          IRepositoryQueryBase<T, K, TContext>
        where T : EntityBase<K>
        where TContext : DbContext
    {
        // This interface intentionally has no additional members.
        // It serves as a unified interface combining:
        // - All CRUD operations from IRepositoryBase<T, K>
        // - All fluent query methods from IRepositoryQueryBase<T, K, TContext>
        // - All simple query operations inherited through both interfaces
    }
}
