# Issue #33: Repository Interface Refactoring - REVISED PLAN (Gemini Alternative A)

## Overview

Refactor the repository interface hierarchy to separate **command operations** from **query operations**, following Gemini's recommended Alternative A design.

## Gemini's Critical Review Findings

✅ **Good Idea** - Architecture improvement is sound
🚨 **Critical Issues Found** in original proposal:
1. Lost fluent methods when using simple inheritance
2. Fluent method "downgrading" - calling Include() loses Add/Update/Delete methods
3. Leaky abstraction with Expression<Func<T, bool>>

💡 **Recommended Solution**: Separate Query and Command Interfaces

## Current Architecture

```
IRepositoryQueryBase<T, K, TContext>
    └── IRepositoryBase<T, K, TContext>

RepositoryQueryBase<T, K, TContext> : IRepositoryQueryBase<T, K, TContext>
    └── RepositoryBase<T, K, TContext> : IRepositoryBase<T, K, TContext>
```

## New Architecture (Gemini Alternative A)

```
IRepositoryQueryBase<T, K>           (NEW - simple queries, context-independent)
    └── IRepositoryQueryBase<T, K, TContext>  (fluent query builder, EF-specific)

IRepositoryBase<T, K>                (NEW - commands + simple queries, context-independent)

GenericRepository<T, K, TContext>    (implements BOTH IRepositoryBase<T,K> AND IRepositoryQueryBase<T,K,TContext>)
```

**Key Difference**: No `IRepositoryBase<T, K, TContext>` interface! The concrete class implements both interfaces directly.

## Benefits

1. **Clear Separation**: Commands vs. Queries are distinct concerns
2. **No Method Conflicts**: Fluent query methods don't interfere with command methods
3. **DI Flexibility**:
   - Inject `IRepositoryBase<Product, int>` for CRUD operations
   - Inject `IRepositoryQueryBase<Product, int, MyContext>` for complex queries
4. **Testability**: Mock command operations without EF Core concerns
5. **Future-Proof**: Foundation for Specification pattern or non-EF implementations

## Interface Definitions

### 1. IRepositoryQueryBase<T, K> (Context-Independent Simple Queries)

**File**: `src/EFCore.GenericRepository/Repositories/Interfaces/IRepositoryQueryBase.cs` (NEW FILE)

```csharp
namespace EFCore.GenericRepository.Repositories.Interfaces;

/// <summary>
/// Base interface for simple, context-independent query operations.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
/// <typeparam name="K">The primary key type.</typeparam>
public interface IRepositoryQueryBase<T, K>
    where T : EntityBase<K>
{
    /// <summary>
    /// Gets an entity by its primary key.
    /// </summary>
    Task<T?> GetByIdAsync(K id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an entity by its primary key.
    /// </summary>
    T? GetById(K id);

    /// <summary>
    /// Gets all entities.
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all entities.
    /// </summary>
    IEnumerable<T> GetAll();

    /// <summary>
    /// Finds entities based on a predicate.
    /// </summary>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds entities based on a predicate.
    /// </summary>
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Finds a single entity based on a predicate.
    /// </summary>
    Task<T?> FindSingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds a single entity based on a predicate.
    /// </summary>
    T? FindSingle(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Checks if any entity satisfies a condition.
    /// </summary>
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if any entity satisfies a condition.
    /// </summary>
    bool Any(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Gets the count of entities.
    /// </summary>
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the count of entities.
    /// </summary>
    int Count(Expression<Func<T, bool>>? predicate = null);

    /// <summary>
    /// Gets a paginated list of entities.
    /// </summary>
    Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a paginated list of entities.
    /// </summary>
    PagedResult<T> GetPaged(int pageNumber, int pageSize);
}
```

### 2. IRepositoryBase<T, K> (Context-Independent Commands)

**File**: `src/EFCore.GenericRepository/Repositories/Interfaces/IRepositoryBase.cs` (NEW FILE)

```csharp
namespace EFCore.GenericRepository.Repositories.Interfaces;

/// <summary>
/// Base interface for CRUD operations and simple queries, without DbContext dependency.
/// For complex, fluent queries, use <see cref="IRepositoryQueryBase{T, K, TContext}"/>.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
/// <typeparam name="K">The primary key type.</typeparam>
public interface IRepositoryBase<T, K>
    where T : EntityBase<K>
{
    #region Simple Query Operations

    /// <summary>
    /// Gets an entity by its primary key.
    /// </summary>
    Task<T?> GetByIdAsync(K id, CancellationToken cancellationToken = default);
    T? GetById(K id);

    /// <summary>
    /// Gets all entities.
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    IEnumerable<T> GetAll();

    /// <summary>
    /// Finds entities based on a predicate.
    /// </summary>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

    #endregion

    #region Add Operations

    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    T Add(T entity);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    void AddRange(IEnumerable<T> entities);

    #endregion

    #region Update Operations

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    T Update(T entity);
    void UpdateRange(IEnumerable<T> entities);

    #endregion

    #region Delete Operations

    /// <summary>
    /// Deletes an entity (soft delete if ISoftDeletable, otherwise hard delete).
    /// </summary>
    Task DeleteAsync(K id, CancellationToken cancellationToken = default);
    void Delete(K id);
    void Delete(T entity);
    Task DeleteRangeAsync(IEnumerable<K> ids, CancellationToken cancellationToken = default);
    void DeleteRange(IEnumerable<K> ids);
    void DeleteRange(IEnumerable<T> entities);

    #endregion

    #region Hard Delete Operations

    /// <summary>
    /// Permanently deletes an entity, regardless of ISoftDeletable.
    /// </summary>
    void HardDelete(T entity);
    void HardDeleteRange(IEnumerable<T> entities);

    #endregion

    #region Restore Operations

    /// <summary>
    /// Restores a soft-deleted entity.
    /// </summary>
    Task RestoreAsync(K id, CancellationToken cancellationToken = default);
    void Restore(T entity);

    #endregion
}
```

### 3. IRepositoryQueryBase<T, K, TContext> (EF-Specific Fluent Queries)

**File**: Rename existing `IRepositoryQueryBase.cs` to `IRepositoryQueryBaseWithContext.cs`

```csharp
namespace EFCore.GenericRepository.Repositories.Interfaces;

/// <summary>
/// Query builder interface with DbContext-specific fluent operations (Include, AsNoTracking, OrderBy).
/// Extends <see cref="IRepositoryQueryBase{T, K}"/> with EF Core-specific query building capabilities.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
/// <typeparam name="K">The primary key type.</typeparam>
/// <typeparam name="TContext">The DbContext type.</typeparam>
public interface IRepositoryQueryBase<T, K, TContext> : IRepositoryQueryBase<T, K>
    where T : EntityBase<K>
    where TContext : DbContext
{
    /// <summary>
    /// Includes a navigation property for eager loading.
    /// </summary>
    IRepositoryQueryBase<T, K, TContext> Include(Expression<Func<T, object>> includeExpression);

    /// <summary>
    /// Disables change tracking for read-only queries.
    /// </summary>
    IRepositoryQueryBase<T, K, TContext> AsNoTracking();

    /// <summary>
    /// Sorts entities in ascending order.
    /// </summary>
    IRepositoryQueryBase<T, K, TContext> OrderBy(Expression<Func<T, object>> keySelector);

    /// <summary>
    /// Sorts entities in descending order.
    /// </summary>
    IRepositoryQueryBase<T, K, TContext> OrderByDescending(Expression<Func<T, object>> keySelector);

    /// <summary>
    /// Performs subsequent ordering in ascending order.
    /// </summary>
    IRepositoryQueryBase<T, K, TContext> ThenBy(Expression<Func<T, object>> keySelector);

    /// <summary>
    /// Performs subsequent ordering in descending order.
    /// </summary>
    IRepositoryQueryBase<T, K, TContext> ThenByDescending(Expression<Func<T, object>> keySelector);
}
```

### 4. NO IRepositoryBase<T, K, TContext> Interface!

**Key Insight from Gemini**: We don't need this interface. The concrete class implements both:
- `IRepositoryBase<T, K>` for commands
- `IRepositoryQueryBase<T, K, TContext>` for fluent queries

## Implementation Changes

### RepositoryQueryBase<T, K, TContext>

```csharp
public abstract class RepositoryQueryBase<T, K, TContext>
    : IRepositoryQueryBase<T, K, TContext>
    where T : EntityBase<K>
    where TContext : DbContext
{
    // Implements all methods from IRepositoryQueryBase<T, K> (simple queries)
    // Implements all methods from IRepositoryQueryBase<T, K, TContext> (fluent queries)
}
```

### RepositoryBase<T, K, TContext>

```csharp
public class RepositoryBase<T, K, TContext>
    : RepositoryQueryBase<T, K, TContext>,
      IRepositoryBase<T, K>  // Explicitly implement command interface
    where T : EntityBase<K>
    where TContext : DbContext
{
    // Inherits query methods from RepositoryQueryBase
    // Implements command methods from IRepositoryBase<T, K>
}
```

## DI Registration Strategy

### ServiceCollectionExtensions Update

```csharp
public static IServiceCollection AddGenericRepository<TContext>(
    this IServiceCollection services)
    where TContext : DbContext
{
    // Register concrete repository type
    services.AddScoped(typeof(RepositoryBase<,,>));

    // Forward IRepositoryBase<T, K> requests to concrete type
    services.AddScoped(typeof(IRepositoryBase<,>), provider =>
    {
        // Resolve from concrete repository
        var repoType = typeof(RepositoryBase<,,>).MakeGenericType(
            typeof(T), typeof(K), typeof(TContext));
        return provider.GetRequiredService(repoType);
    });

    // Forward IRepositoryQueryBase<T, K, TContext> requests to concrete type
    services.AddScoped(typeof(IRepositoryQueryBase<,,>), provider =>
    {
        var repoType = typeof(RepositoryBase<,,>).MakeGenericType(
            typeof(T), typeof(K), typeof(TContext));
        return provider.GetRequiredService(repoType);
    });

    // Register UnitOfWork
    services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();

    return services;
}
```

## Usage Patterns

### Pattern 1: Simple CRUD Operations (Context-Independent)

```csharp
public class ProductService
{
    private readonly IRepositoryBase<Product, int> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(
        IRepositoryBase<Product, int> repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        await _repository.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
        return product;
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await _repository.FindAsync(p => p.IsActive);
    }
}
```

### Pattern 2: Complex Queries with Fluent API

```csharp
public class ProductQueryService
{
    private readonly IRepositoryQueryBase<Product, int, AppDbContext> _queryRepository;

    public ProductQueryService(
        IRepositoryQueryBase<Product, int, AppDbContext> queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<PagedResult<Product>> GetProductsWithCategoryAsync(
        int pageNumber, int pageSize)
    {
        return await _queryRepository
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .GetPagedAsync(pageNumber, pageSize);
    }
}
```

### Pattern 3: Using UnitOfWork (Existing Pattern Maintained)

```csharp
public class OrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task CreateOrderAsync(Order order)
    {
        // UnitOfWork returns IRepositoryBase<T, K>
        var orderRepo = _unitOfWork.Repository<Order, int>();
        await orderRepo.AddAsync(order);

        var productRepo = _unitOfWork.Repository<Product, int>();
        // ... update product inventory

        await _unitOfWork.SaveChangesAsync();
    }
}
```

## Migration Impact

### Breaking Changes
- ✅ **NONE for existing code using UnitOfWork** - continues to work as-is
- ✅ Services using old interfaces can continue (we'll keep them temporarily)

### New Capabilities
- ✅ Can inject `IRepositoryBase<T, K>` for simple, testable CRUD
- ✅ Can inject `IRepositoryQueryBase<T, K, TContext>` for complex queries
- ✅ Clear separation improves code organization

### Migration Path
1. Keep old `IRepositoryBase<T, K, TContext>` interface temporarily (mark `[Obsolete]`)
2. Gradually refactor services to use new interfaces
3. Remove obsolete interface in v2.0.0

## Testing Strategy

### Unit Tests Updates

1. **IRepositoryBase<T, K> Tests**
   - Mock without DbContext
   - Test all CRUD operations
   - Verify simple query operations

2. **IRepositoryQueryBase<T, K, TContext> Tests**
   - Test fluent query building
   - Verify Include, AsNoTracking, OrderBy chains
   - Test pagination with sorting

3. **Integration Tests**
   - Verify both interfaces work with same concrete type
   - Test DI registration resolves correctly
   - End-to-end workflows

## Success Criteria

- ✅ All 130+ tests passing
- ✅ No breaking changes to UnitOfWork usage
- ✅ New DI patterns work correctly
- ✅ Sample application demonstrates both usage patterns
- ✅ Clean separation between commands and queries
- ✅ No fluent method downgrading issues

## Estimated Effort

- Interface creation: 1-2 hours
- Implementation updates: 2-3 hours
- DI registration: 1 hour
- Test updates: 2-3 hours
- Sample updates: 1 hour
- Documentation: 1 hour
- **Total: 8-10 hours**

## References

- Gemini Review: `.claude/skills/gemini-subagent-skill/gemini_response.md`
- Original Proposal: `REFACTORING-PLAN-ISSUE-33.md`
- GitHub Issue: Issue #33 in GitHub-Issues.md
