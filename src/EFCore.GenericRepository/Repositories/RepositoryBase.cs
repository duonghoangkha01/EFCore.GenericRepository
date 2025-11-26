using EFCore.GenericRepository.Entities;
using EFCore.GenericRepository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EFCore.GenericRepository.Repositories
{
    /// <summary>
    /// Base implementation for repository operations with full CRUD support.
    /// Provides automatic soft delete functionality for entities implementing <see cref="ISoftDeletable"/>.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="K">The entity's primary key type.</typeparam>
    /// <typeparam name="TContext">The DbContext type.</typeparam>
    public abstract class RepositoryBase<T, K, TContext> : RepositoryQueryBase<T, K, TContext>, IRepositoryBase<T, K, TContext>
        where T : EntityBase<K>
        where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{T, K, TContext}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        protected RepositoryBase(TContext dbContext) : base(dbContext)
        {
        }

        #region Add Operations

        /// <inheritdoc />
        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var entry = await DbContext.Set<T>().AddAsync(entity, cancellationToken);
            return entry.Entity;
        }

        /// <inheritdoc />
        public virtual T Add(T entity)
        {
            var entry = DbContext.Set<T>().Add(entity);
            return entry.Entity;
        }

        /// <inheritdoc />
        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await DbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
        }

        /// <inheritdoc />
        public virtual void AddRange(IEnumerable<T> entities)
        {
            DbContext.Set<T>().AddRange(entities);
        }

        #endregion

        #region Update Operations

        /// <inheritdoc />
        public virtual T Update(T entity)
        {
            var entry = DbContext.Set<T>().Update(entity);
            return entry.Entity;
        }

        /// <inheritdoc />
        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            DbContext.Set<T>().UpdateRange(entities);
        }

        #endregion

        #region Delete Operations (Soft Delete Support)

        /// <inheritdoc />
        public virtual async Task DeleteAsync(K id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, cancellationToken);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        /// <inheritdoc />
        public virtual void Delete(K id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        /// <inheritdoc />
        public virtual void Delete(T entity)
        {
            // Check if entity implements ISoftDeletable
            if (entity is ISoftDeletable softDeletableEntity)
            {
                // Soft delete: mark as deleted
                softDeletableEntity.IsDeleted = true;
                softDeletableEntity.DeletedAt = DateTimeOffset.UtcNow;
                Update(entity);
            }
            else
            {
                // Hard delete: permanently remove
                DbContext.Set<T>().Remove(entity);
            }
        }

        /// <inheritdoc />
        public virtual async Task DeleteRangeAsync(IEnumerable<K> ids, CancellationToken cancellationToken = default)
        {
            foreach (var id in ids)
            {
                await DeleteAsync(id, cancellationToken);
            }
        }

        /// <inheritdoc />
        public virtual void DeleteRange(IEnumerable<K> ids)
        {
            foreach (var id in ids)
            {
                Delete(id);
            }
        }

        /// <inheritdoc />
        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        #endregion

        #region Hard Delete Operations

        /// <inheritdoc />
        public virtual void HardDelete(T entity)
        {
            DbContext.Set<T>().Remove(entity);
        }

        /// <inheritdoc />
        public virtual void HardDeleteRange(IEnumerable<T> entities)
        {
            DbContext.Set<T>().RemoveRange(entities);
        }

        #endregion

        #region Restore Operations

        /// <inheritdoc />
        public virtual async Task RestoreAsync(K id, CancellationToken cancellationToken = default)
        {
            // For soft-deletable entities, we need to query including deleted entities
            // This requires temporarily disabling the global query filter
            var entity = await DbContext.Set<T>()
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(e => EqualityComparer<K>.Default.Equals(e.Id, id), cancellationToken);

            if (entity != null)
            {
                Restore(entity);
            }
        }

        /// <inheritdoc />
        public virtual void Restore(K id)
        {
            // For soft-deletable entities, we need to query including deleted entities
            // This requires temporarily disabling the global query filter
            var entity = DbContext.Set<T>()
                .IgnoreQueryFilters()
                .SingleOrDefault(e => EqualityComparer<K>.Default.Equals(e.Id, id));

            if (entity != null)
            {
                Restore(entity);
            }
        }

        /// <inheritdoc />
        public virtual void Restore(T entity)
        {
            // Check if entity implements ISoftDeletable
            if (entity is ISoftDeletable softDeletableEntity)
            {
                // Restore: unmark as deleted
                softDeletableEntity.IsDeleted = false;
                softDeletableEntity.DeletedAt = null;
                Update(entity);
            }
            // If not soft-deletable, do nothing (can't restore what wasn't soft-deleted)
        }

        #endregion
    }
}
