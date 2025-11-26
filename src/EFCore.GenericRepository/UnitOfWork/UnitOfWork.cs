using EFCore.GenericRepository.Entities;
using EFCore.GenericRepository.Repositories;
using EFCore.GenericRepository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EFCore.GenericRepository.UnitOfWork
{
    /// <summary>
    /// Implementation of the Unit of Work pattern.
    /// Manages repositories and coordinates the work of multiple repositories by creating a single database context shared by all of them.
    /// </summary>
    /// <typeparam name="TContext">The DbContext type.</typeparam>
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private IDbContextTransaction? _currentTransaction;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork{TContext}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <exception cref="ArgumentNullException">Thrown when context is null.</exception>
        public UnitOfWork(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _repositories = new Dictionary<Type, object>();
        }

        /// <inheritdoc />
        public TContext Context => _context;

        #region Repository Access

        /// <inheritdoc />
        public IRepositoryBase<T, K, TContext> Repository<T, K>() where T : EntityBase<K>
        {
            var entityType = typeof(T);

            // Check if repository is already cached
            if (_repositories.ContainsKey(entityType))
            {
                return (IRepositoryBase<T, K, TContext>)_repositories[entityType];
            }

            // Create new repository instance
            // We create a concrete RepositoryBase<T, K, TContext> instance
            // This uses an anonymous implementation since RepositoryBase is abstract
            var repository = new RepositoryImplementation<T, K, TContext>(_context);

            // Cache the repository
            _repositories.Add(entityType, repository);

            return repository;
        }

        /// <summary>
        /// Internal concrete implementation of RepositoryBase for use by UnitOfWork.
        /// This allows UnitOfWork to instantiate repositories without requiring custom repository classes.
        /// </summary>
        private class RepositoryImplementation<T, K, TContext> : RepositoryBase<T, K, TContext>
            where T : EntityBase<K>
            where TContext : DbContext
        {
            public RepositoryImplementation(TContext dbContext) : base(dbContext)
            {
            }
        }

        #endregion

        #region SaveChanges Operations

        /// <inheritdoc />
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        /// <inheritdoc />
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        #endregion

        #region Transaction Management

        /// <inheritdoc />
        public IDbContextTransaction BeginTransaction()
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress. Commit or rollback the current transaction before starting a new one.");
            }

            _currentTransaction = _context.Database.BeginTransaction();
            return _currentTransaction;
        }

        /// <inheritdoc />
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress. Commit or rollback the current transaction before starting a new one.");
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            return _currentTransaction;
        }

        /// <inheritdoc />
        public void Commit()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No transaction is in progress. Call BeginTransaction before committing.");
            }

            try
            {
                _currentTransaction.Commit();
            }
            finally
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }

        /// <inheritdoc />
        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No transaction is in progress. Call BeginTransactionAsync before committing.");
            }

            try
            {
                await _currentTransaction.CommitAsync(cancellationToken);
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        /// <inheritdoc />
        public void Rollback()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No transaction is in progress. Call BeginTransaction before rolling back.");
            }

            try
            {
                _currentTransaction.Rollback();
            }
            finally
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }

        /// <inheritdoc />
        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No transaction is in progress. Call BeginTransactionAsync before rolling back.");
            }

            try
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        #endregion

        #region Disposal

        /// <summary>
        /// Releases the resources used by this unit of work.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by this unit of work and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose current transaction if any
                    if (_currentTransaction != null)
                    {
                        _currentTransaction.Dispose();
                        _currentTransaction = null;
                    }

                    // Clear repository cache
                    _repositories.Clear();

                    // Dispose context
                    _context?.Dispose();
                }

                _disposed = true;
            }
        }

        #endregion
    }
}
