using EFCore.GenericRepository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EFCore.GenericRepository.Repositories.Interfaces
{
    /// <summary>
    /// Represents the base interface for read-only repository operations.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="K">The entity's primary key type.</typeparam>
    /// <typeparam name="TContext">The DbContext type.</typeparam>
    public interface IRepositoryQueryBase<T, K, TContext> where T : EntityBase<K> where TContext : DbContext
    {
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

        /// <summary>
        /// Includes a navigation property to be eager-loaded.
        /// </summary>
        /// <param name="includeExpression">The expression representing the navigation property to include.</param>
        /// <returns>The repository query object with the included navigation property.</returns>
        IRepositoryQueryBase<T, K, TContext> Include(Expression<Func<T, object>> includeExpression);

        /// <summary>
        /// Disables change tracking for the query.
        /// </summary>
        /// <returns>The repository query object with change tracking disabled.</returns>
        IRepositoryQueryBase<T, K, TContext> AsNoTracking();

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>The repository query object with sorting applied.</returns>
        IRepositoryQueryBase<T, K, TContext> OrderBy(Expression<Func<T, object>> keySelector);

        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key.
        /// </summary>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>The repository query object with sorting applied.</returns>
        IRepositoryQueryBase<T, K, TContext> OrderByDescending(Expression<Func<T, object>> keySelector);

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending order.
        /// </summary>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>The repository query object with sorting applied.</returns>
        IRepositoryQueryBase<T, K, TContext> ThenBy(Expression<Func<T, object>> keySelector);

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in descending order.
        /// </summary>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>The repository query object with sorting applied.</returns>
        IRepositoryQueryBase<T, K, TContext> ThenByDescending(Expression<Func<T, object>> keySelector);

        /// <summary>
        /// Gets a paginated list of entities.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A paginated result of entities.</returns>
        Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a paginated list of entities.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A paginated result of entities.</returns>
        PagedResult<T> GetPaged(int pageNumber, int pageSize);
    }
}
