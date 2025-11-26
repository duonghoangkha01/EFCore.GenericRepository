using EFCore.GenericRepository.Entities;
using EFCore.GenericRepository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EFCore.GenericRepository.UnitOfWork
{
    /// <summary>
    /// Defines the contract for the Unit of Work pattern.
    /// Coordinates the work of multiple repositories by creating a single database context shared by all of them.
    /// Ensures that all repository operations are part of a single transaction.
    /// </summary>
    /// <typeparam name="TContext">The DbContext type.</typeparam>
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        /// <summary>
        /// Gets the database context associated with this unit of work.
        /// </summary>
        TContext Context { get; }

        #region Repository Access

        /// <summary>
        /// Gets a repository for the specified entity type.
        /// Repositories are cached and reused within the same unit of work instance.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <typeparam name="K">The entity's primary key type.</typeparam>
        /// <returns>A repository instance for the specified entity type.</returns>
        /// <remarks>
        /// This method uses lazy initialization and caching. Multiple calls with the same type parameters
        /// will return the same repository instance.
        /// </remarks>
        IRepositoryBase<T, K, TContext> Repository<T, K>() where T : EntityBase<K>;

        #endregion

        #region SaveChanges Operations

        /// <summary>
        /// Saves all changes made in this unit of work to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        /// <remarks>
        /// This method delegates to <see cref="DbContext.SaveChanges()"/>.
        /// Any changes tracked by repositories within this unit of work will be persisted.
        /// </remarks>
        int SaveChanges();

        /// <summary>
        /// Asynchronously saves all changes made in this unit of work to the database.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous save operation.
        /// The task result contains the number of state entries written to the database.
        /// </returns>
        /// <remarks>
        /// This method delegates to <see cref="DbContext.SaveChangesAsync(CancellationToken)"/>.
        /// Any changes tracked by repositories within this unit of work will be persisted.
        /// </remarks>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        #endregion

        #region Transaction Management

        /// <summary>
        /// Begins a new database transaction.
        /// </summary>
        /// <returns>A transaction object representing the newly created transaction.</returns>
        /// <remarks>
        /// Use this method when you need explicit transaction control.
        /// Remember to call <see cref="Commit"/> or <see cref="Rollback"/> to complete the transaction.
        /// The transaction should be disposed after use.
        /// </remarks>
        IDbContextTransaction BeginTransaction();

        /// <summary>
        /// Asynchronously begins a new database transaction.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a transaction object representing the newly created transaction.
        /// </returns>
        /// <remarks>
        /// Use this method when you need explicit transaction control with async operations.
        /// Remember to call <see cref="CommitAsync"/> or <see cref="RollbackAsync"/> to complete the transaction.
        /// The transaction should be disposed after use.
        /// </remarks>
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        /// <remarks>
        /// This method commits all changes made within the current transaction to the database.
        /// The transaction must have been previously started with <see cref="BeginTransaction"/>.
        /// </remarks>
        void Commit();

        /// <summary>
        /// Asynchronously commits the current transaction.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous commit operation.</returns>
        /// <remarks>
        /// This method commits all changes made within the current transaction to the database.
        /// The transaction must have been previously started with <see cref="BeginTransactionAsync"/>.
        /// </remarks>
        Task CommitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        /// <remarks>
        /// This method discards all changes made within the current transaction.
        /// The transaction must have been previously started with <see cref="BeginTransaction"/>.
        /// </remarks>
        void Rollback();

        /// <summary>
        /// Asynchronously rolls back the current transaction.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous rollback operation.</returns>
        /// <remarks>
        /// This method discards all changes made within the current transaction.
        /// The transaction must have been previously started with <see cref="BeginTransactionAsync"/>.
        /// </remarks>
        Task RollbackAsync(CancellationToken cancellationToken = default);

        #endregion
    }
}
