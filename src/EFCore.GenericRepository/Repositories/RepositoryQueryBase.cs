using EFCore.GenericRepository.Entities;
using EFCore.GenericRepository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EFCore.GenericRepository.Repositories
{
    /// <summary>
    /// Base implementation for read-only repository operations.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="K">The entity's primary key type.</typeparam>
    /// <typeparam name="TContext">The DbContext type.</typeparam>
    public abstract class RepositoryQueryBase<T, K, TContext> : IRepositoryQueryBase<T, K, TContext>
        where T : EntityBase<K>
        where TContext : DbContext
    {
        /// <summary>
        /// The database context.
        /// </summary>
        protected readonly TContext DbContext;
        
        /// <summary>
        /// The queryable entity set.
        /// </summary>
        protected IQueryable<T> Query;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryQueryBase{T, K, TContext}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        protected RepositoryQueryBase(TContext dbContext)
        {
            DbContext = dbContext;
            Query = DbContext.Set<T>();
        }

        /// <inheritdoc />
        public virtual T? GetById(K id)
        {
            return Query.SingleOrDefault(e => EqualityComparer<K>.Default.Equals(e.Id, id));
        }

        /// <inheritdoc />
        public virtual async Task<T?> GetByIdAsync(K id, CancellationToken cancellationToken = default)
        {
            return await Query.SingleOrDefaultAsync(e => EqualityComparer<K>.Default.Equals(e.Id, id), cancellationToken);
        }

        /// <inheritdoc />
        public virtual IEnumerable<T> GetAll()
        {
            return Query.ToList();
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await Query.ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return Query.Where(predicate).ToList();
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await Query.Where(predicate).ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual T? FindSingle(Expression<Func<T, bool>> predicate)
        {
            return Query.SingleOrDefault(predicate);
        }

        /// <inheritdoc />
        public virtual async Task<T?> FindSingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await Query.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        /// <inheritdoc />
        public virtual bool Any(Expression<Func<T, bool>> predicate)
        {
            return Query.Any(predicate);
        }

        /// <inheritdoc />
        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await Query.AnyAsync(predicate, cancellationToken);
        }

        /// <inheritdoc />
        public virtual int Count(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate == null ? Query.Count() : Query.Count(predicate);
        }

        /// <inheritdoc />
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            return predicate == null ? await Query.CountAsync(cancellationToken) : await Query.CountAsync(predicate, cancellationToken);
        }

        /// <inheritdoc />
        public virtual PagedResult<T> GetPaged(int pageNumber, int pageSize)
        {
            var totalCount = Query.Count();
            var items = Query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        /// <inheritdoc />
        public virtual async Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var totalCount = await Query.CountAsync(cancellationToken);
            var items = await Query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        /// <inheritdoc />
        public virtual IRepositoryQueryBase<T, K, TContext> Include(Expression<Func<T, object>> includeExpression)
        {
            Query = Query.Include(includeExpression);
            return this;
        }

        /// <inheritdoc />
        public virtual IRepositoryQueryBase<T, K, TContext> AsNoTracking()
        {
            Query = Query.AsNoTracking();
            return this;
        }

        /// <inheritdoc />
        public virtual IRepositoryQueryBase<T, K, TContext> OrderBy(Expression<Func<T, object>> keySelector)
        {
            Query = Query.OrderBy(keySelector);
            return this;
        }

        /// <inheritdoc />
        public virtual IRepositoryQueryBase<T, K, TContext> OrderByDescending(Expression<Func<T, object>> keySelector)
        {
            Query = Query.OrderByDescending(keySelector);
            return this;
        }

        /// <inheritdoc />
        public virtual IRepositoryQueryBase<T, K, TContext> ThenBy(Expression<Func<T, object>> keySelector)
        {
            if (Query is IOrderedQueryable<T> orderedQuery)
            {
                Query = orderedQuery.ThenBy(keySelector);
            }
            else
            {
                // Handle case where OrderBy has not been called before ThenBy
                // Potentially throw an exception or just apply OrderBy
                throw new InvalidOperationException("ThenBy can only be applied to a sorted query. Use OrderBy first.");
            }
            return this;
        }

        /// <inheritdoc />
        public virtual IRepositoryQueryBase<T, K, TContext> ThenByDescending(Expression<Func<T, object>> keySelector)
        {
            if (Query is IOrderedQueryable<T> orderedQuery)
            {
                Query = orderedQuery.ThenByDescending(keySelector);
            }
            else
            {
                // Handle case where OrderBy has not been called before ThenByDescending
                throw new InvalidOperationException("ThenByDescending can only be applied to a sorted query. Use OrderBy first.");
            }
            return this;
        }
    }
}