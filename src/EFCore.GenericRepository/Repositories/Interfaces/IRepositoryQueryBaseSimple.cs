using EFCore.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EFCore.GenericRepository.Repositories.Interfaces
{
    /// <summary>
    /// Base interface for simple, context-independent query operations.
    /// For complex queries with Include, AsNoTracking, and OrderBy, use <see cref="IRepositoryQueryBase{T, K, TContext}"/>.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="K">The entity's primary key type.</typeparam>
    /// <remarks>
    /// This interface defines query operations that are independent of the DbContext type,
    /// making it easier to create testable services and allowing for potential non-EF implementations.
    /// Services that only need basic CRUD and simple queries can depend on this interface
    /// instead of the EF Core-specific <see cref="IRepositoryQueryBase{T, K, TContext}"/>.
    /// </remarks>
    public interface IRepositoryQueryBase<T, K>
        where T : EntityBase<K>
    {
        #region Get By ID

        /// <summary>
        /// Gets an entity by its primary key.
        /// </summary>
        /// <param name="id">The primary key of the entity.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The entity, or null if not found.</returns>
        Task<T?> GetByIdAsync(K id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets an entity by its primary key.
        /// </summary>
        /// <param name="id">The primary key of the entity.</param>
        /// <returns>The entity, or null if not found.</returns>
        T? GetById(K id);

        #endregion

        #region Get All

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A collection of all entities.</returns>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>A collection of all entities.</returns>
        IEnumerable<T> GetAll();

        #endregion

        #region Find

        /// <summary>
        /// Finds entities based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A collection of entities that match the predicate.</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds entities based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <returns>A collection of entities that match the predicate.</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        #endregion

        #region Find Single

        /// <summary>
        /// Finds a single entity based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The first entity that matches the predicate, or null if not found.</returns>
        Task<T?> FindSingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds a single entity based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <returns>The first entity that matches the predicate, or null if not found.</returns>
        T? FindSingle(Expression<Func<T, bool>> predicate);

        #endregion

        #region Any / Exists

        /// <summary>
        /// Checks if any entity satisfies a condition.
        /// </summary>
        /// <param name="predicate">The predicate to check.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>True if any entity satisfies the condition, otherwise false.</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if any entity satisfies a condition.
        /// </summary>
        /// <param name="predicate">The predicate to check.</param>
        /// <returns>True if any entity satisfies the condition, otherwise false.</returns>
        bool Any(Expression<Func<T, bool>> predicate);

        #endregion

        #region Count

        /// <summary>
        /// Gets the count of entities.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities. If null, counts all entities.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The total number of entities.</returns>
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the count of entities.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities. If null, counts all entities.</param>
        /// <returns>The total number of entities.</returns>
        int Count(Expression<Func<T, bool>>? predicate = null);

        #endregion

        #region Pagination

        /// <summary>
        /// Gets a paginated list of entities.
        /// </summary>
        /// <param name="pageNumber">The page number (1-based).</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A paginated result of entities with metadata.</returns>
        Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a paginated list of entities.
        /// </summary>
        /// <param name="pageNumber">The page number (1-based).</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A paginated result of entities with metadata.</returns>
        PagedResult<T> GetPaged(int pageNumber, int pageSize);

        #endregion
    }
}
