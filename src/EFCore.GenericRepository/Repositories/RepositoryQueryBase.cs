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
        protected readonly TContext DbContext;
        protected IQueryable<T> Query;

        protected RepositoryQueryBase(TContext dbContext)
        {
            DbContext = dbContext;
            Query = DbContext.Set<T>();
        }

        public virtual T? GetById(K id)
        {
            return Query.SingleOrDefault(e => e.Id.Equals(id));
        }

        public virtual async Task<T?> GetByIdAsync(K id, CancellationToken cancellationToken = default)
        {
            return await Query.SingleOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Query.ToList();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await Query.ToListAsync(cancellationToken);
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return Query.Where(predicate).ToList();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await Query.Where(predicate).ToListAsync(cancellationToken);
        }

        public virtual T? FindSingle(Expression<Func<T, bool>> predicate)
        {
            return Query.SingleOrDefault(predicate);
        }

        public virtual async Task<T?> FindSingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await Query.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual bool Any(Expression<Func<T, bool>> predicate)
        {
            return Query.Any(predicate);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await Query.AnyAsync(predicate, cancellationToken);
        }

        public virtual int Count(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate == null ? Query.Count() : Query.Count(predicate);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            return predicate == null ? await Query.CountAsync(cancellationToken) : await Query.CountAsync(predicate, cancellationToken);
        }

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

        public virtual IRepositoryQueryBase<T, K, TContext> Include(Expression<Func<T, object>> includeExpression)
        {
            Query = Query.Include(includeExpression);
            return this;
        }

        public virtual IRepositoryQueryBase<T, K, TContext> AsNoTracking()
        {
            Query = Query.AsNoTracking();
            return this;
        }

        public virtual IRepositoryQueryBase<T, K, TContext> OrderBy(Expression<Func<T, object>> keySelector)
        {
            Query = Query.OrderBy(keySelector);
            return this;
        }

        public virtual IRepositoryQueryBase<T, K, TContext> OrderByDescending(Expression<Func<T, object>> keySelector)
        {
            Query = Query.OrderByDescending(keySelector);
            return this;
        }

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
