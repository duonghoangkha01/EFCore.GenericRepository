using EFCore.GenericRepository.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EFCore.GenericRepository.Repositories.Interfaces
{
    /// <summary>
    /// Represents the base interface for repository operations with full CRUD support.
    /// Extends <see cref="IRepositoryQueryBase{T, K, TContext}"/> with write operations.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="K">The entity's primary key type.</typeparam>
    /// <typeparam name="TContext">The DbContext type.</typeparam>
    public interface IRepositoryBase<T, K, TContext> : IRepositoryQueryBase<T, K, TContext>
        where T : EntityBase<K>
        where TContext : DbContext
    {
        #region Add Operations

        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The added entity.</returns>
        /// <remarks>
        /// This method does not save changes to the database. Call <see cref="DbContext.SaveChanges"/> or
        /// <see cref="DbContext.SaveChangesAsync"/> to persist changes.
        /// </remarks>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        /// <remarks>
        /// This method does not save changes to the database. Call <see cref="DbContext.SaveChanges"/> to persist changes.
        /// </remarks>
        T Add(T entity);

        /// <summary>
        /// Adds a collection of entities to the repository.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method does not save changes to the database. Call <see cref="DbContext.SaveChanges"/> or
        /// <see cref="DbContext.SaveChangesAsync"/> to persist changes.
        /// </remarks>
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a collection of entities to the repository.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <remarks>
        /// This method does not save changes to the database. Call <see cref="DbContext.SaveChanges"/> to persist changes.
        /// </remarks>
        void AddRange(IEnumerable<T> entities);

        #endregion

        #region Update Operations

        /// <summary>
        /// Updates an existing entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The updated entity.</returns>
        /// <remarks>
        /// This method marks the entity as modified in the change tracker. Call <see cref="DbContext.SaveChanges"/>
        /// or <see cref="DbContext.SaveChangesAsync"/> to persist changes.
        /// </remarks>
        T Update(T entity);

        /// <summary>
        /// Updates a collection of entities in the repository.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        /// <remarks>
        /// This method marks the entities as modified in the change tracker. Call <see cref="DbContext.SaveChanges"/>
        /// or <see cref="DbContext.SaveChangesAsync"/> to persist changes.
        /// </remarks>
        void UpdateRange(IEnumerable<T> entities);

        #endregion

        #region Delete Operations (Soft Delete)

        /// <summary>
        /// Deletes an entity by its primary key.
        /// </summary>
        /// <param name="id">The primary key of the entity to delete.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// <para>If the entity implements <see cref="ISoftDeletable"/>, it will be soft deleted (marked as deleted but not removed from database).</para>
        /// <para>Otherwise, the entity will be permanently removed from the database.</para>
        /// <para>Call <see cref="DbContext.SaveChanges"/> or <see cref="DbContext.SaveChangesAsync"/> to persist changes.</para>
        /// </remarks>
        Task DeleteAsync(K id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an entity by its primary key.
        /// </summary>
        /// <param name="id">The primary key of the entity to delete.</param>
        /// <remarks>
        /// <para>If the entity implements <see cref="ISoftDeletable"/>, it will be soft deleted (marked as deleted but not removed from database).</para>
        /// <para>Otherwise, the entity will be permanently removed from the database.</para>
        /// <para>Call <see cref="DbContext.SaveChanges"/> to persist changes.</para>
        /// </remarks>
        void Delete(K id);

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <remarks>
        /// <para>If the entity implements <see cref="ISoftDeletable"/>, it will be soft deleted (marked as deleted but not removed from database).</para>
        /// <para>Otherwise, the entity will be permanently removed from the database.</para>
        /// <para>Call <see cref="DbContext.SaveChanges"/> or <see cref="DbContext.SaveChangesAsync"/> to persist changes.</para>
        /// </remarks>
        void Delete(T entity);

        /// <summary>
        /// Deletes multiple entities by their primary keys.
        /// </summary>
        /// <param name="ids">The primary keys of the entities to delete.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// <para>If entities implement <see cref="ISoftDeletable"/>, they will be soft deleted (marked as deleted but not removed from database).</para>
        /// <para>Otherwise, entities will be permanently removed from the database.</para>
        /// <para>Call <see cref="DbContext.SaveChanges"/> or <see cref="DbContext.SaveChangesAsync"/> to persist changes.</para>
        /// </remarks>
        Task DeleteRangeAsync(IEnumerable<K> ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes multiple entities by their primary keys.
        /// </summary>
        /// <param name="ids">The primary keys of the entities to delete.</param>
        /// <remarks>
        /// <para>If entities implement <see cref="ISoftDeletable"/>, they will be soft deleted (marked as deleted but not removed from database).</para>
        /// <para>Otherwise, entities will be permanently removed from the database.</para>
        /// <para>Call <see cref="DbContext.SaveChanges"/> to persist changes.</para>
        /// </remarks>
        void DeleteRange(IEnumerable<K> ids);

        /// <summary>
        /// Deletes multiple entities.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        /// <remarks>
        /// <para>If entities implement <see cref="ISoftDeletable"/>, they will be soft deleted (marked as deleted but not removed from database).</para>
        /// <para>Otherwise, entities will be permanently removed from the database.</para>
        /// <para>Call <see cref="DbContext.SaveChanges"/> or <see cref="DbContext.SaveChangesAsync"/> to persist changes.</para>
        /// </remarks>
        void DeleteRange(IEnumerable<T> entities);

        #endregion

        #region Hard Delete Operations

        /// <summary>
        /// Permanently deletes an entity from the database, regardless of whether it implements <see cref="ISoftDeletable"/>.
        /// </summary>
        /// <param name="entity">The entity to permanently delete.</param>
        /// <remarks>
        /// <para>This method forces permanent deletion even for entities implementing <see cref="ISoftDeletable"/>.</para>
        /// <para>Use with caution as this operation cannot be undone.</para>
        /// <para>Call <see cref="DbContext.SaveChanges"/> or <see cref="DbContext.SaveChangesAsync"/> to persist changes.</para>
        /// </remarks>
        void HardDelete(T entity);

        /// <summary>
        /// Permanently deletes multiple entities from the database, regardless of whether they implement <see cref="ISoftDeletable"/>.
        /// </summary>
        /// <param name="entities">The entities to permanently delete.</param>
        /// <remarks>
        /// <para>This method forces permanent deletion even for entities implementing <see cref="ISoftDeletable"/>.</para>
        /// <para>Use with caution as this operation cannot be undone.</para>
        /// <para>Call <see cref="DbContext.SaveChanges"/> or <see cref="DbContext.SaveChangesAsync"/> to persist changes.</para>
        /// </remarks>
        void HardDeleteRange(IEnumerable<T> entities);

        #endregion

        #region Restore Operations

        /// <summary>
        /// Restores a soft-deleted entity by its primary key.
        /// </summary>
        /// <param name="id">The primary key of the entity to restore.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// <para>This method only works for entities implementing <see cref="ISoftDeletable"/>.</para>
        /// <para>If the entity does not implement <see cref="ISoftDeletable"/> or is not found, no action is taken.</para>
        /// <para>Call <see cref="DbContext.SaveChanges"/> or <see cref="DbContext.SaveChangesAsync"/> to persist changes.</para>
        /// </remarks>
        Task RestoreAsync(K id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Restores a soft-deleted entity by its primary key.
        /// </summary>
        /// <param name="id">The primary key of the entity to restore.</param>
        /// <remarks>
        /// <para>This method only works for entities implementing <see cref="ISoftDeletable"/>.</para>
        /// <para>If the entity does not implement <see cref="ISoftDeletable"/> or is not found, no action is taken.</para>
        /// <para>Call <see cref="DbContext.SaveChanges"/> to persist changes.</para>
        /// </remarks>
        void Restore(K id);

        /// <summary>
        /// Restores a soft-deleted entity.
        /// </summary>
        /// <param name="entity">The entity to restore.</param>
        /// <remarks>
        /// <para>This method only works for entities implementing <see cref="ISoftDeletable"/>.</para>
        /// <para>If the entity does not implement <see cref="ISoftDeletable"/>, no action is taken.</para>
        /// <para>Call <see cref="DbContext.SaveChanges"/> or <see cref="DbContext.SaveChangesAsync"/> to persist changes.</para>
        /// </remarks>
        void Restore(T entity);

        #endregion
    }
}
