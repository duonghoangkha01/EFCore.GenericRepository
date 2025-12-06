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
    /// Query builder interface with DbContext-specific fluent operations (Include, AsNoTracking, OrderBy).
    /// Extends <see cref="IRepositoryQueryBase{T, K}"/> with EF Core-specific query building capabilities.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="K">The entity's primary key type.</typeparam>
    /// <typeparam name="TContext">The DbContext type.</typeparam>
    /// <remarks>
    /// <para>
    /// This interface extends the context-independent <see cref="IRepositoryQueryBase{T, K}"/> with EF Core-specific
    /// fluent query building methods. Use this when you need:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Eager loading with Include()</description></item>
    /// <item><description>Change tracking control with AsNoTracking()</description></item>
    /// <item><description>Complex sorting with OrderBy(), ThenBy(), etc.</description></item>
    /// </list>
    /// <para>
    /// Simple query operations (GetById, GetAll, Find, Count, etc.) are inherited from the base interface.
    /// </para>
    /// </remarks>
    public interface IRepositoryQueryBase<T, K, TContext> : IRepositoryQueryBase<T, K>
        where T : EntityBase<K>
        where TContext : DbContext
    {
        #region Fluent Query Building (EF Core Specific)

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

        #endregion

        // Note: Simple query methods (GetById, GetAll, Find, FindSingle, Any, Count, GetPaged)
        // are inherited from IRepositoryQueryBase<T, K> base interface.
    }
}
