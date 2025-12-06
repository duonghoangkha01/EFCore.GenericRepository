# Issue #33: Repository Interface Refactoring - Detailed Plan

## Overview

Refactor the repository interface hierarchy to create a cleaner separation between context-independent contracts and context-specific implementations.

## Current Architecture

```
IRepositoryQueryBase<T, K, TContext>
    └── IRepositoryBase<T, K, TContext>

RepositoryQueryBase<T, K, TContext> : IRepositoryQueryBase<T, K, TContext>
    └── RepositoryBase<T, K, TContext> : IRepositoryBase<T, K, TContext>
```

## New Architecture

```
IRepositoryQueryBase<T, K>                    (NEW - context-independent)
    ├── IRepositoryBase<T, K>                 (NEW - context-independent)
    │       └── IRepositoryBase<T, K, TContext>
    └── IRepositoryQueryBase<T, K, TContext>
            └── IRepositoryBase<T, K, TContext>

RepositoryQueryBase<T, K, TContext> : IRepositoryQueryBase<T, K, TContext>
    └── RepositoryBase<T, K, TContext> : IRepositoryBase<T, K, TContext>
```

## Benefits

1. **Separation of Concerns**: Context-independent contracts vs. context-specific implementations
2. **Dependency Injection Flexibility**: Can inject `IRepositoryBase<Product, int>` without TContext
3. **SOLID Principles**: Interface Segregation Principle - clients depend only on what they need
4. **Future-Proof**: Foundation for non-EF implementations (Dapper, raw ADO.NET, etc.)
5. **Testability**: Easier to mock without DbContext concerns

## Implementation Steps

### Step 1: Create Context-Independent Query Interface

**File**: `src/EFCore.GenericRepository/Repositories/Interfaces/IRepositoryQueryBase.cs`

```csharp
namespace EFCore.GenericRepository.Repositories.Interfaces;

/// <summary>
/// Base interface for read-only repository operations without DbContext dependency.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
/// <typeparam name="K">The primary key type.</typeparam>
public interface IRepositoryQueryBase<T, K>
    where T : EntityBase<K>
{
    // Get by ID
    Task<T?> GetByIdAsync(K id, CancellationToken cancellationToken = default);
    T? GetById(K id);

    // Get all
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    IEnumerable<T> GetAll();

    // Find with predicate
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

    // Find single
    Task<T?> FindSingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    T? FindSingle(Expression<Func<T, bool>> predicate);

    // Exists/Any
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    bool Any(Expression<Func<T, bool>> predicate);

    // Count
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    int Count(Expression<Func<T, bool>>? predicate = null);

    // Pagination
    Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    PagedResult<T> GetPaged(int pageNumber, int pageSize);
}
```

### Step 2: Create Context-Independent CRUD Interface

**File**: `src/EFCore.GenericRepository/Repositories/Interfaces/IRepositoryBase.cs` (rename existing)

```csharp
namespace EFCore.GenericRepository.Repositories.Interfaces;

/// <summary>
/// Base interface for full CRUD repository operations without DbContext dependency.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
/// <typeparam name="K">The primary key type.</typeparam>
public interface IRepositoryBase<T, K> : IRepositoryQueryBase<T, K>
    where T : EntityBase<K>
{
    // Add operations
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    T Add(T entity);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    void AddRange(IEnumerable<T> entities);

    // Update operations
    T Update(T entity);
    void UpdateRange(IEnumerable<T> entities);

    // Delete operations (soft or hard based on ISoftDeletable)
    Task DeleteAsync(K id, CancellationToken cancellationToken = default);
    void Delete(K id);
    void Delete(T entity);
    Task DeleteRangeAsync(IEnumerable<K> ids, CancellationToken cancellationToken = default);
    void DeleteRange(IEnumerable<K> ids);
    void DeleteRange(IEnumerable<T> entities);

    // Hard delete operations
    void HardDelete(T entity);
    void HardDeleteRange(IEnumerable<T> entities);

    // Restore operations
    Task RestoreAsync(K id, CancellationToken cancellationToken = default);
    void Restore(T entity);
}
```

### Step 3: Create Context-Specific Query Interface

**File**: `src/EFCore.GenericRepository/Repositories/Interfaces/IRepositoryQueryBaseWithContext.cs` (new name)

```csharp
namespace EFCore.GenericRepository.Repositories.Interfaces;

/// <summary>
/// Query interface with DbContext-specific operations (Include, AsNoTracking, OrderBy).
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
/// <typeparam name="K">The primary key type.</typeparam>
/// <typeparam name="TContext">The DbContext type.</typeparam>
public interface IRepositoryQueryBase<T, K, TContext> : IRepositoryQueryBase<T, K>
    where T : EntityBase<K>
    where TContext : DbContext
{
    // Include navigation properties (EF Core specific)
    IRepositoryQueryBase<T, K, TContext> Include(Expression<Func<T, object>> includeExpression);

    // No tracking (EF Core specific)
    IRepositoryQueryBase<T, K, TContext> AsNoTracking();

    // Sorting (fluent interface with TContext)
    IRepositoryQueryBase<T, K, TContext> OrderBy(Expression<Func<T, object>> keySelector);
    IRepositoryQueryBase<T, K, TContext> OrderByDescending(Expression<Func<T, object>> keySelector);
    IRepositoryQueryBase<T, K, TContext> ThenBy(Expression<Func<T, object>> keySelector);
    IRepositoryQueryBase<T, K, TContext> ThenByDescending(Expression<Func<T, object>> keySelector);
}
```

### Step 4: Update Context-Specific CRUD Interface

**File**: Update existing `IRepositoryBase<T, K, TContext>`

```csharp
namespace EFCore.GenericRepository.Repositories.Interfaces;

/// <summary>
/// Full CRUD repository interface with DbContext-specific operations.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
/// <typeparam name="K">The primary key type.</typeparam>
/// <typeparam name="TContext">The DbContext type.</typeparam>
public interface IRepositoryBase<T, K, TContext> : IRepositoryBase<T, K>
    where T : EntityBase<K>
    where TContext : DbContext
{
    // Inherits all CRUD operations from IRepositoryBase<T, K>
    // Inherits query operations from IRepositoryQueryBase<T, K> (through IRepositoryBase<T, K>)

    // Add DbContext-specific fluent methods by returning the context-specific interface
    new IRepositoryQueryBase<T, K, TContext> Include(Expression<Func<T, object>> includeExpression);
    new IRepositoryQueryBase<T, K, TContext> AsNoTracking();
    new IRepositoryQueryBase<T, K, TContext> OrderBy(Expression<Func<T, object>> keySelector);
    new IRepositoryQueryBase<T, K, TContext> OrderByDescending(Expression<Func<T, object>> keySelector);
    new IRepositoryQueryBase<T, K, TContext> ThenBy(Expression<Func<T, object>> keySelector);
    new IRepositoryQueryBase<T, K, TContext> ThenByDescending(Expression<Func<T, object>> keySelector);
}
```

### Step 5: Update Repository Implementations

**No changes needed** - implementations already implement `IRepositoryBase<T, K, TContext>`, which now inherits from the new base interfaces.

### Step 6: Update UnitOfWork Interface

**File**: `src/EFCore.GenericRepository/UnitOfWork/IUnitOfWork.cs`

```csharp
// Option 1: Keep existing signature (recommended for backwards compatibility)
IRepositoryBase<T, K, TContext> Repository<T, K>()
    where T : EntityBase<K>;

// Option 2: Return context-independent interface (breaking change)
IRepositoryBase<T, K> Repository<T, K>()
    where T : EntityBase<K>;

// Recommendation: Keep Option 1, add Option 2 as additional overload if needed
```

### Step 7: File Organization

**Current Structure**:
```
src/EFCore.GenericRepository/Repositories/Interfaces/
├── IRepositoryQueryBase.cs              (currently has TContext)
└── IRepositoryBase.cs                   (currently has TContext)
```

**New Structure**:
```
src/EFCore.GenericRepository/Repositories/Interfaces/
├── IRepositoryQueryBase.cs              (NEW - without TContext)
├── IRepositoryBase.cs                   (NEW - without TContext)
├── IRepositoryQueryBaseWithContext.cs   (with TContext)
└── IRepositoryBaseWithContext.cs        (with TContext)
```

**Alternative (Better)**:
```
src/EFCore.GenericRepository/Repositories/Interfaces/
├── IRepositoryQueryBase.cs              (both versions via partial or nested namespace)
└── IRepositoryBase.cs                   (both versions via partial or nested namespace)
```

**Recommended Approach**: Keep file names simple, use generic type parameters to differentiate:
- `IRepositoryQueryBase<T, K>` and `IRepositoryQueryBase<T, K, TContext>` in same file
- `IRepositoryBase<T, K>` and `IRepositoryBase<T, K, TContext>` in same file

## Testing Strategy

### Unit Tests to Update

1. **RepositoryQueryBaseTests.cs**
   - Verify it implements both `IRepositoryQueryBase<T, K>` and `IRepositoryQueryBase<T, K, TContext>`
   - Test context-independent methods work

2. **RepositoryBaseTests.cs**
   - Verify it implements both `IRepositoryBase<T, K>` and `IRepositoryBase<T, K, TContext>`
   - Test all CRUD operations still work

3. **UnitOfWorkTests.cs**
   - Verify Repository<T, K>() returns correct interface
   - Test repository caching still works

### New Tests to Add

1. **Interface hierarchy tests**
   - Verify inheritance chain is correct
   - Test that context-independent interfaces can be mocked without DbContext

## Migration Impact Analysis

### Breaking Changes
- ✅ **None expected** - All existing code should continue to work
- Implementations still return `IRepositoryBase<T, K, TContext>`
- UnitOfWork API remains unchanged

### Internal Changes
- New base interfaces added
- Existing interfaces inherit from new bases
- No changes to implementations or public API

### Consumer Code
```csharp
// This continues to work exactly as before
var repo = _unitOfWork.Repository<Product, int>();
await repo.AddAsync(product);
await _unitOfWork.SaveChangesAsync();

// NEW: Can now inject context-independent interface if desired
public class ProductService
{
    private readonly IRepositoryBase<Product, int> _repo; // No TContext needed!

    public ProductService(IRepositoryBase<Product, int> repo)
    {
        _repo = repo;
    }
}
```

## Documentation Updates

### XML Documentation
- Add comprehensive docs to new interfaces
- Explain difference between context-independent and context-specific interfaces
- Provide usage examples

### README Updates
- Document new interface hierarchy
- Show when to use context-independent vs. context-specific interfaces
- Update architecture diagram

## Rollout Plan

### Phase 1: Create New Interfaces (Non-Breaking)
1. Create `IRepositoryQueryBase<T, K>`
2. Create `IRepositoryBase<T, K>`
3. Update existing interfaces to inherit from new bases
4. **Result**: No breaking changes, all tests pass

### Phase 2: Update Tests
1. Update all repository tests
2. Update UnitOfWork tests
3. Add new interface hierarchy tests
4. **Result**: Full test coverage of new structure

### Phase 3: Update Samples
1. Update console sample
2. Show usage of context-independent interfaces
3. **Result**: Samples demonstrate new capabilities

### Phase 4: Documentation
1. Update XML comments
2. Update README
3. Update architecture diagrams
4. **Result**: Complete documentation

## Success Criteria

- ✅ All 130+ tests passing
- ✅ Build succeeds with no warnings
- ✅ Sample applications run successfully
- ✅ No breaking changes to existing consumer code
- ✅ New interfaces properly documented
- ✅ Code coverage maintained ≥ 80%

## Estimated Effort

- **Interface Creation**: 1-2 hours
- **Implementation Updates**: 1 hour
- **Test Updates**: 2-3 hours
- **Sample Updates**: 1 hour
- **Documentation**: 1-2 hours
- **Total**: 6-9 hours

## Risk Assessment

**Low Risk** - This is primarily an additive change:
- New base interfaces added
- Existing interfaces inherit from them
- No changes to implementations
- No breaking changes to public API

**Mitigation**:
- Run full test suite after each change
- Verify samples still work
- Check for any compilation warnings
