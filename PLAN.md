# EFCore.GenericRepository - Detailed Implementation Plan

> **📌 Note for Claude Code Sessions**
> This plan file serves as the single source of truth for the project. When starting a new conversation:
> 1. Read the "PROJECT STATUS & PROGRESS TRACKING" section to understand current state
> 2. Check "Current Task" to see what needs to be done next
> 3. Review "Session History" to understand what's been completed
> 4. Update this file after completing any task to maintain continuity
> 5. Always update "Last Updated", "Current Status", and "Overall Progress"

---

## 📊 PROJECT STATUS & PROGRESS TRACKING

**Last Updated**: 2025-11-26
**Current Status**: 🟢 **IN PROGRESS** - Initial setup completed
**Overall Progress**: 15.6% (5/32 issues completed)

### ✅ Completed Tasks
- [x] Create detailed implementation plan
- [x] Finalize all architecture decisions (Q1-Q8)
- [x] Add versioning and publishing strategy
- [x] Add GitHub repository and task management section
- [x] Create GitHub Issues task list

### 🔄 Current Task
**Task**: Implementing Issue #6: Implement ISoftDeletable interface
**Status**: Pending user decision
**Blockers**: None
**Next Action**: Start implementing Issue #6

### 📋 Next Steps (In Order)
1. [x] Create GitHub repository: `EFCore.GenericRepository`
2. [x] Create 32 GitHub Issues with labels and milestones
3. [x] Set up project board and branch protection
4. [x] Create local solution and project structure
5. [x] Initialize git repository and push to GitHub
6. [x] Start implementing Issue #1: Initial repository structure

### 📝 Session History

#### Session 1 - 2025-11-25 - Planning Phase
- Created comprehensive implementation plan
- Made all architecture decisions:
  - Framework: .NET 6 + EF Core 8.0
  - Library Name: EFCore.GenericRepository
  - Features: DI extensions, rich pagination, sorting support, soft delete
  - Both sync and async methods
- Added versioning strategy (Semantic Versioning)
- Added publishing strategy (GitHub Actions + NuGet)
- Created 32 GitHub Issues organized into 3 milestones
- Created issue templates and project board structure
- **Status**: Plan complete, ready for implementation

#### Session 2 - 2025-11-26 - Initial Setup Completion
- Completed Issue #1: Create initial repository structure and .gitignore
- Completed Issue #2: Configure project properties and NuGet metadata
- Completed Issue #3: Create README.md with initial documentation
- Completed Issue #4: Set up CI/CD pipeline (GitHub Actions)
- **Status**: Initial setup complete, proceeding to core implementation

#### Session 3 - 2025-11-26 - Core Entities
- Completed Issue #5: Implement EntityBase<K> class
- **Status**: Core entity base class implemented and tested.

---

## 🎯 Quick Reference for New Sessions

### What's Been Decided
- **Target Framework**: .NET 6
- **EF Core Version**: 8.0
- **Package Name**: EFCore.GenericRepository
- **Repository**: Public on GitHub
- **License**: MIT

### Key Features to Implement
1. EntityBase<K> with generic key support
2. ISoftDeletable interface for soft delete
3. PagedResult<T> for rich pagination
4. IRepositoryQueryBase with query operations
5. RepositoryQueryBase implementation
6. IRepositoryBase with CRUD operations
7. RepositoryBase implementation
8. IUnitOfWork interface
9. UnitOfWork<TContext> implementation
10. DbContextExtensions for soft delete configuration
11. ServiceCollectionExtensions for DI registration

### Implementation Order (See Section 6)
- **Phase 1**: Core Setup (Issues #1-4)
- **Phase 2**: Entity Foundation (Issues #5-7)
- **Phase 3**: Query Layer (Issues #8-10)
- **Phase 4**: Repository Layer (Issues #11-13)
- **Phase 5**: Unit of Work (Issues #14-16)
- **Phase 6**: Extensions (Issues #17-18)
- **Phase 7**: Samples & Tests (Issues #19-22)
- **Phase 8**: Documentation (Issues #23-26)
- **Phase 9**: Publishing (Issues #27-28)
- **Phase 10**: Release (Issues #29-32)

### Important Files
- **Main Plan**: `GenericRepository-Library-Plan.md` (this file)
- **GitHub Issues**: `GitHub-Issues-Tasks.md`
- **Project Location**: TBD (user to specify)
- **GitHub Repo**: TBD (to be created)

### Current Blockers
- None - waiting for user to confirm:
  1. GitHub username
  2. Local project path (e.g., D:\Repos\EFCore.GenericRepository)
  3. Whether to use automated setup (gh CLI) or manual

---

## 1. Project Overview

### Purpose
Create a reusable .NET library providing generic repository pattern implementation with Entity Framework Core support, featuring:
- Generic CRUD operations
- Unit of Work pattern
- Soft delete functionality
- Query capabilities with filtering, sorting, and pagination
- Async/await support throughout

### Target Audience
.NET developers using Entity Framework Core who want to avoid repetitive repository code across multiple projects.

---

## 2. Architecture Decisions

### 2.1 Framework Target
**DECISION: .NET 6 + EF Core 8.0** ✓

Chosen for modern features with good compatibility:
- Works with .NET 6, .NET 7, .NET 8+
- Long-term support (LTS) - .NET 6 supported until November 2024
- Latest EF Core 8.0 features and performance improvements
- Modern C# language features (C# 10+)
- Still broadly compatible with current .NET ecosystem

**Target Framework**: `net6.0`
**EF Core Version**: `8.0.0` (latest stable)

### 2.2 Project Naming
**DECISION: `EFCore.GenericRepository`** ✓

Why this name:
- Clear and descriptive - immediately conveys purpose
- Follows .NET naming conventions
- Excellent NuGet discoverability
- Professional and memorable
- Indicates EF Core dependency upfront

**Package ID**: `EFCore.GenericRepository`
**Root Namespace**: `EFCore.GenericRepository`
**Assembly Name**: `EFCore.GenericRepository`

### 2.3 Soft Delete Strategy
- Use marker interface `ISoftDeletable`
- Add `IsDeleted` (bool) and `DeletedAt` (DateTime?) properties
- Automatic query filtering via global query filters
- Manual override option for queries that need deleted records

---

## 3. Project Structure

```
EFCore.GenericRepository/
├── src/
│   └── EFCore.GenericRepository/
│       ├── Entities/
│       │   ├── EntityBase.cs
│       │   ├── ISoftDeletable.cs
│       │   └── PagedResult.cs
│       ├── Repositories/
│       │   ├── Interfaces/
│       │   │   ├── IRepositoryQueryBase.cs
│       │   │   └── IRepositoryBase.cs
│       │   ├── RepositoryQueryBase.cs
│       │   └── RepositoryBase.cs
│       ├── UnitOfWork/
│       │   ├── IUnitOfWork.cs
│       │   └── UnitOfWork.cs
│       ├── Extensions/
│       │   ├── DbContextExtensions.cs
│       │   └── ServiceCollectionExtensions.cs
│       └── EFCore.GenericRepository.csproj
├── tests/
│   └── EFCore.GenericRepository.Tests/
│       ├── Fixtures/
│       ├── RepositoryTests.cs
│       ├── UnitOfWorkTests.cs
│       └── EFCore.GenericRepository.Tests.csproj
├── samples/
│   └── EFCore.GenericRepository.Sample/
│       └── EFCore.GenericRepository.Sample.csproj
├── EFCore.GenericRepository.sln
└── README.md
```

---

## 4. Component Design

### 4.1 EntityBase&lt;K&gt;

**Purpose**: Base class for all entities with generic primary key type.

```csharp
public abstract class EntityBase<K>
{
    public K Id { get; set; }
}
```

**Key Points**:
- Generic key type `K` supports different key types (int, long, Guid, string, etc.)
- Abstract class to prevent direct instantiation
- Minimal design - only essential properties

---

### 4.2 ISoftDeletable

**Purpose**: Marker interface for entities supporting soft delete.

```csharp
public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
}
```

**Key Points**:
- Entities implementing this interface will have soft delete functionality
- Optional - only implement if entity needs soft delete
- `DeletedAt` tracks when deletion occurred

---

### 4.3 PagedResult&lt;T&gt;

**Purpose**: Result type for paginated queries with metadata.

```csharp
public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}
```

**Key Points**:
- Contains both data and pagination metadata
- `TotalPages` automatically calculated from TotalCount and PageSize
- `HasPreviousPage` and `HasNextPage` for UI convenience
- Makes building pagination controls in UIs straightforward

---

### 4.4 IRepositoryQueryBase&lt;T, K, TContext&gt;

**Purpose**: Interface defining read-only repository operations.

**Methods**:
```csharp
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

// Include navigation properties
IRepositoryQueryBase<T, K, TContext> Include(Expression<Func<T, object>> includeExpression);

// No tracking (for read-only scenarios)
IRepositoryQueryBase<T, K, TContext> AsNoTracking();

// Sorting
IRepositoryQueryBase<T, K, TContext> OrderBy(Expression<Func<T, object>> keySelector);
IRepositoryQueryBase<T, K, TContext> OrderByDescending(Expression<Func<T, object>> keySelector);
IRepositoryQueryBase<T, K, TContext> ThenBy(Expression<Func<T, object>> keySelector);
IRepositoryQueryBase<T, K, TContext> ThenByDescending(Expression<Func<T, object>> keySelector);

// Pagination with metadata
Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
PagedResult<T> GetPaged(int pageNumber, int pageSize);
```

**Key Points**:
- Both sync and async versions
- Fluent interface for Include, AsNoTracking, and sorting
- Cancellation token support
- Rich pagination with metadata (PagedResult<T>)
- Sorting with OrderBy/ThenBy support

---

### 4.4 RepositoryQueryBase&lt;T, K, TContext&gt;

**Purpose**: Base implementation of read operations.

**Key Features**:
- Protected `DbSet<T>` property
- Lazy-loaded query building
- Include chain support
- Global query filter for soft delete (automatic filtering of `IsDeleted = true`)

**Implementation Notes**:
- Use `IQueryable<T>` internally for composable queries
- Apply `AsNoTracking()` appropriately for read-only operations
- Soft delete filtering applied by default, with option to include deleted

---

### 4.5 IRepositoryBase&lt;T, K, TContext&gt;

**Purpose**: Interface extending query interface with write operations.

**Additional Methods**:
```csharp
// Add
Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
T Add(T entity);
Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
void AddRange(IEnumerable<T> entities);

// Update
T Update(T entity);
void UpdateRange(IEnumerable<T> entities);

// Delete (soft or hard based on ISoftDeletable)
Task DeleteAsync(K id, CancellationToken cancellationToken = default);
void Delete(K id);
void Delete(T entity);
Task DeleteRangeAsync(IEnumerable<K> ids, CancellationToken cancellationToken = default);
void DeleteRange(IEnumerable<K> ids);
void DeleteRange(IEnumerable<T> entities);

// Hard delete (force permanent deletion even for soft-deletable entities)
void HardDelete(T entity);
void HardDeleteRange(IEnumerable<T> entities);

// Restore soft-deleted entities
Task RestoreAsync(K id, CancellationToken cancellationToken = default);
void Restore(T entity);
```

---

### 4.6 RepositoryBase&lt;T, K, TContext&gt;

**Purpose**: Complete repository implementation with CRUD operations.

**Key Features**:
- Inherits from `RepositoryQueryBase<T, K, TContext>`
- Implements `IRepositoryBase<T, K, TContext>`
- Automatic soft delete handling:
  - If entity implements `ISoftDeletable`, `Delete()` sets `IsDeleted = true` and `DeletedAt = DateTime.UtcNow`
  - Otherwise, performs hard delete
- `HardDelete()` methods always remove from database

**Implementation Notes**:
- Does NOT call `SaveChanges()` - delegates to Unit of Work
- Uses reflection or type checking to determine if entity is soft-deletable
- All write operations work with DbContext change tracker

---

### 4.7 IUnitOfWork

**Purpose**: Coordinate repository operations and transaction management.

```csharp
public interface IUnitOfWork : IDisposable
{
    // Get repository for entity type
    IRepositoryBase<T, K, TContext> Repository<T, K>()
        where T : EntityBase<K>
        where TContext : DbContext;

    // Save changes
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();

    // Transaction management
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    IDbContextTransaction BeginTransaction();
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    void CommitTransaction();
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    void RollbackTransaction();
}
```

**Key Points**:
- Single DbContext instance per Unit of Work
- Repository instances cached per entity type
- Transaction support
- Proper disposal of DbContext

---

### 4.8 UnitOfWork Implementation

**Purpose**: Concrete implementation of IUnitOfWork.

**Key Features**:
- Generic constructor accepting `TContext` (DbContext)
- Dictionary to cache repository instances
- Lazy repository creation
- Transaction management via DbContext
- Proper disposal pattern

**Implementation Notes**:
```csharp
public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _context;
    private readonly Dictionary<Type, object> _repositories;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(TContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }

    // Repository creation and caching
    // SaveChanges delegation
    // Transaction methods
    // Dispose pattern
}
```

---

### 4.9 DbContextExtensions

**Purpose**: Extension methods for DbContext to configure soft delete.

```csharp
public static class DbContextExtensions
{
    public static void ConfigureSoftDelete(this ModelBuilder modelBuilder)
    {
        // Apply global query filter for all ISoftDeletable entities
        // Filter: entity.IsDeleted == false
    }
}
```

**Usage**:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    modelBuilder.ConfigureSoftDelete();
}
```

---

### 4.10 ServiceCollectionExtensions

**Purpose**: Extension methods for dependency injection registration.

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGenericRepository<TContext>(
        this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
        return services;
    }

    // Optional: with DbContext configuration
    public static IServiceCollection AddGenericRepository<TContext>(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> optionsAction)
        where TContext : DbContext
    {
        services.AddDbContext<TContext>(optionsAction);
        services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
        return services;
    }
}
```

**Usage**:
```csharp
// Simple registration (DbContext already registered)
services.AddGenericRepository<AppDbContext>();

// With DbContext configuration
services.AddGenericRepository<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
```

---

## 5. GitHub Repository & Task Management

### 5.1 GitHub Repository Setup

**Repository Details**:
- **Name**: `EFCore.GenericRepository`
- **Description**: Generic Repository pattern implementation with Unit of Work for Entity Framework Core
- **Visibility**: Public
- **License**: MIT
- **Topics/Tags**: `csharp`, `dotnet`, `entity-framework-core`, `repository-pattern`, `unit-of-work`, `generic-repository`, `nuget-package`

**Initial Repository Structure**:
```
.github/
├── workflows/
│   ├── ci.yml
│   └── publish-nuget.yml
├── ISSUE_TEMPLATE/
│   ├── bug_report.md
│   ├── feature_request.md
│   └── question.md
└── PULL_REQUEST_TEMPLATE.md
.gitignore
LICENSE
README.md
CHANGELOG.md
CONTRIBUTING.md
CODE_OF_CONDUCT.md
```

### 5.2 Issue Templates

**Bug Report Template** (`.github/ISSUE_TEMPLATE/bug_report.md`):
```markdown
---
name: Bug Report
about: Report a bug or unexpected behavior
title: '[BUG] '
labels: bug
assignees: ''
---

**Describe the bug**
A clear description of what the bug is.

**To Reproduce**
Steps to reproduce:
1. ...
2. ...

**Expected behavior**
What you expected to happen.

**Code Sample**
```csharp
// Minimal code to reproduce the issue
```

**Environment**
- EFCore.GenericRepository version:
- .NET version:
- EF Core version:
- Database provider:

**Additional context**
Any other relevant information.
```

**Feature Request Template** (`.github/ISSUE_TEMPLATE/feature_request.md`):
```markdown
---
name: Feature Request
about: Suggest a new feature
title: '[FEATURE] '
labels: enhancement
assignees: ''
---

**Feature Description**
Clear description of the feature.

**Use Case**
Why is this feature needed? What problem does it solve?

**Proposed Solution**
How should this feature work?

**Code Example**
```csharp
// Example of how the feature would be used
```

**Alternatives Considered**
Other approaches you've considered.
```

### 5.3 GitHub Project Board

**Create Project**: "EFCore.GenericRepository v1.0"

**Columns**:
1. **Backlog** - Future tasks
2. **To Do** - Ready to start
3. **In Progress** - Currently working
4. **Review** - Code review/testing
5. **Done** - Completed

**Milestones**:
- `v1.0.0-alpha` - Core functionality
- `v1.0.0-beta` - Feature complete, testing
- `v1.0.0` - Production release

### 5.4 GitHub Issues/Tasks List

**Milestone: v1.0.0-alpha**

#### Setup & Infrastructure
- [ ] **#1** - Create initial repository structure and .gitignore
  - Labels: `setup`, `priority: high`
  - Create solution, project structure
  - Add .gitignore for .NET
  - Set up directory structure

- [ ] **#2** - Configure project properties and NuGet metadata
  - Labels: `setup`, `priority: high`
  - Add package metadata to .csproj
  - Configure target framework (net6.0)
  - Add package dependencies

- [ ] **#3** - Create README.md with initial documentation
  - Labels: `documentation`, `priority: high`
  - Project overview
  - Installation instructions (placeholder)
  - Basic usage example
  - Contribution guidelines link

- [ ] **#4** - Set up CI/CD pipeline (GitHub Actions)
  - Labels: `ci-cd`, `priority: high`
  - Create ci.yml workflow
  - Configure build and test jobs
  - Add multi-version .NET testing

#### Core Entities & Interfaces
- [ ] **#5** - Implement EntityBase<K> class
  - Labels: `core`, `priority: high`
  - Create base entity with generic key
  - Add XML documentation
  - Unit tests

- [ ] **#6** - Implement ISoftDeletable interface
  - Labels: `core`, `priority: high`
  - Create interface with IsDeleted and DeletedAt
  - Add XML documentation

- [ ] **#7** - Implement PagedResult<T> class
  - Labels: `core`, `priority: high`
  - Create PagedResult with metadata
  - Add calculated properties (HasNextPage, etc.)
  - Add XML documentation
  - Unit tests

#### Repository Query Layer
- [ ] **#8** - Define IRepositoryQueryBase<T, K, TContext> interface
  - Labels: `repository`, `priority: high`
  - Define all query methods
  - Include Include(), AsNoTracking()
  - Add sorting methods (OrderBy, ThenBy)
  - Add pagination method
  - Add XML documentation

- [ ] **#9** - Implement RepositoryQueryBase<T, K, TContext> class
  - Labels: `repository`, `priority: high`
  - Implement all query operations
  - Implement fluent interface (Include, OrderBy)
  - Add soft delete filtering
  - Add XML documentation

- [ ] **#10** - Add unit tests for RepositoryQueryBase
  - Labels: `tests`, `priority: high`
  - Test GetById, GetAll, Find
  - Test Include and AsNoTracking
  - Test sorting methods
  - Test pagination
  - Test soft delete filtering

#### Repository CRUD Layer
- [ ] **#11** - Define IRepositoryBase<T, K, TContext> interface
  - Labels: `repository`, `priority: high`
  - Define all CRUD methods
  - Add HardDelete methods
  - Add Restore methods
  - Add XML documentation

- [ ] **#12** - Implement RepositoryBase<T, K, TContext> class
  - Labels: `repository`, `priority: high`
  - Implement Add, Update, Delete operations
  - Implement soft delete logic
  - Implement HardDelete methods
  - Add XML documentation

- [ ] **#13** - Add unit tests for RepositoryBase CRUD operations
  - Labels: `tests`, `priority: high`
  - Test Add/AddRange
  - Test Update/UpdateRange
  - Test Delete (soft delete)
  - Test HardDelete
  - Test Restore

#### Unit of Work
- [ ] **#14** - Define IUnitOfWork interface
  - Labels: `unit-of-work`, `priority: high`
  - Define Repository<T, K>() method
  - Define SaveChanges methods
  - Define transaction methods
  - Add XML documentation

- [ ] **#15** - Implement UnitOfWork<TContext> class
  - Labels: `unit-of-work`, `priority: high`
  - Implement repository caching
  - Implement SaveChanges
  - Implement transaction support
  - Implement disposal pattern
  - Add XML documentation

- [ ] **#16** - Add unit tests for UnitOfWork
  - Labels: `tests`, `priority: high`
  - Test repository creation and caching
  - Test SaveChanges
  - Test transaction commit/rollback
  - Test disposal

#### Extensions
- [ ] **#17** - Implement DbContextExtensions for soft delete
  - Labels: `extensions`, `priority: high`
  - Create ConfigureSoftDelete method
  - Apply global query filters
  - Add XML documentation
  - Unit tests

- [ ] **#18** - Implement ServiceCollectionExtensions for DI
  - Labels: `extensions`, `priority: high`
  - Create AddGenericRepository<TContext> method
  - Add overload with DbContext configuration
  - Add XML documentation

#### Sample Project
- [ ] **#19** - Create sample console application
  - Labels: `sample`, `priority: medium`
  - Create sample entities
  - Create sample DbContext
  - Demonstrate basic CRUD operations
  - Demonstrate pagination and sorting
  - Demonstrate soft delete
  - Add comprehensive comments

- [ ] **#20** - Create sample ASP.NET Core Web API project
  - Labels: `sample`, `priority: low`
  - Create Web API using the library
  - Show DI registration
  - Create CRUD endpoints
  - Add Swagger documentation

#### Integration Tests
- [ ] **#21** - Create integration test project
  - Labels: `tests`, `priority: medium`
  - Set up in-memory database
  - Create test fixtures
  - End-to-end workflow tests

- [ ] **#22** - Add integration tests for complete workflows
  - Labels: `tests`, `priority: medium`
  - Test complete CRUD workflow
  - Test transaction scenarios
  - Test multiple repositories in UnitOfWork
  - Test soft delete end-to-end

**Milestone: v1.0.0-beta**

#### Documentation
- [ ] **#23** - Complete XML documentation for all public APIs
  - Labels: `documentation`, `priority: high`
  - Review all XML comments
  - Add code examples in remarks
  - Document exceptions

- [ ] **#24** - Create comprehensive README.md
  - Labels: `documentation`, `priority: high`
  - Installation guide
  - Quick start
  - Full usage examples
  - API reference
  - FAQ
  - Contributing guidelines

- [ ] **#25** - Create CONTRIBUTING.md
  - Labels: `documentation`, `priority: medium`
  - How to contribute
  - Code style guidelines
  - PR process

- [ ] **#26** - Create CHANGELOG.md
  - Labels: `documentation`, `priority: high`
  - Set up changelog structure
  - Document v1.0.0 changes

#### Publishing
- [ ] **#27** - Set up NuGet publishing workflow
  - Labels: `ci-cd`, `publishing`, `priority: high`
  - Create publish-nuget.yml workflow
  - Configure NuGet API key secret
  - Test package build

- [ ] **#28** - Create package icon and documentation images
  - Labels: `documentation`, `priority: low`
  - Design package icon
  - Create architecture diagrams
  - Create usage flow diagrams

**Milestone: v1.0.0**

#### Release
- [ ] **#29** - Final testing and bug fixes
  - Labels: `testing`, `priority: high`
  - Complete testing pass
  - Fix any remaining bugs
  - Performance testing

- [ ] **#30** - Prepare v1.0.0 release
  - Labels: `release`, `priority: high`
  - Update version numbers
  - Finalize CHANGELOG
  - Create release branch
  - Tag release

- [ ] **#31** - Publish to NuGet.org
  - Labels: `release`, `publishing`, `priority: high`
  - Push v1.0.0 tag
  - Verify automated publish
  - Test package installation

- [ ] **#32** - Create GitHub Release with notes
  - Labels: `release`, `priority: high`
  - Write release notes
  - Attach artifacts
  - Announce release

### 5.5 Labels for Issues

**Type Labels**:
- `bug` - Something isn't working
- `enhancement` - New feature or request
- `documentation` - Documentation improvements
- `question` - Questions about usage
- `good first issue` - Good for newcomers
- `help wanted` - Extra attention needed

**Component Labels**:
- `core` - Core entities and interfaces
- `repository` - Repository implementation
- `unit-of-work` - Unit of Work pattern
- `extensions` - Extension methods
- `tests` - Testing related
- `sample` - Sample projects
- `ci-cd` - CI/CD pipelines
- `publishing` - Package publishing

**Priority Labels**:
- `priority: high` - Critical, must have for v1.0
- `priority: medium` - Important, should have
- `priority: low` - Nice to have

**Status Labels**:
- `in progress` - Currently being worked on
- `blocked` - Blocked by dependencies
- `needs review` - Ready for code review

### 5.6 Branch Protection Rules

**For `main` branch**:
- Require pull request reviews (at least 1)
- Require status checks to pass (CI build)
- Require branches to be up to date
- Include administrators in restrictions
- Restrict force pushes
- Restrict deletions

**For `develop` branch**:
- Require status checks to pass (CI build)
- Restrict force pushes

### 5.7 Repository Setup Commands

```bash
# Create repository on GitHub (via gh CLI or web interface)
gh repo create EFCore.GenericRepository --public \
  --description "Generic Repository pattern implementation with Unit of Work for Entity Framework Core" \
  --license MIT

# Clone locally
cd D:\Repos
git clone https://github.com/YOUR_USERNAME/EFCore.GenericRepository.git
cd EFCore.GenericRepository

# Create initial branch structure
git checkout -b develop
git push -u origin develop

# Protect branches (via GitHub UI or gh CLI)
gh api repos/YOUR_USERNAME/EFCore.GenericRepository/branches/main/protection \
  --method PUT \
  --input protection-rules.json
```

---

## 6. Implementation Steps

### Phase 1: Core Setup
1. Create solution and projects
2. Add NuGet package references
3. Configure project properties and metadata

### Phase 2: Entity Foundation
4. Implement `EntityBase<K>`
5. Implement `ISoftDeletable` interface
6. Create `DbContextExtensions` with soft delete configuration

### Phase 3: Query Layer
7. Define `IRepositoryQueryBase<T, K, TContext>`
8. Implement `RepositoryQueryBase<T, K, TContext>`
9. Add unit tests for query operations

### Phase 4: Repository Layer
10. Define `IRepositoryBase<T, K, TContext>`
11. Implement `RepositoryBase<T, K, TContext>`
12. Add soft delete logic
13. Add unit tests for CRUD operations

### Phase 5: Unit of Work
14. Define `IUnitOfWork`
15. Implement `UnitOfWork<TContext>`
16. Add unit tests for Unit of Work

### Phase 6: Testing & Samples
17. Create comprehensive unit tests
18. Create sample project demonstrating usage
19. Integration tests with in-memory database

### Phase 7: Packaging
20. Configure NuGet package metadata
21. Create README with documentation
22. Add XML documentation comments
23. Build and test NuGet package

---

## 6. NuGet Package Configuration

### Package Metadata
```xml
<PropertyGroup>
  <TargetFramework>net6.0</TargetFramework>
  <PackageId>EFCore.GenericRepository</PackageId>
  <Version>1.0.0</Version>
  <Authors>Your Name</Authors>
  <Company>Your Company</Company>
  <Description>Generic Repository pattern implementation with Unit of Work for Entity Framework Core 8.0. Provides reusable base repository with CRUD operations, soft delete support, and query capabilities.</Description>
  <PackageTags>repository;unit-of-work;entity-framework;ef-core;generic-repository;efcore;soft-delete;net6;efcore8</PackageTags>
  <PackageProjectUrl>https://github.com/yourname/efcore-generic-repository</PackageProjectUrl>
  <RepositoryUrl>https://github.com/yourname/efcore-generic-repository</RepositoryUrl>
  <PackageLicenseExpression>MIT</PackageLicenseExpression>
  <GeneratePackageOnBuild>false</GeneratePackageOnBuild>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <LangVersion>latest</LangVersion>
  <Nullable>enable</Nullable>
  <ImplicitUsings>enable</ImplicitUsings>
</PropertyGroup>
```

### Dependencies
- `Microsoft.EntityFrameworkCore` (8.0.0)
- `Microsoft.EntityFrameworkCore.Relational` (8.0.0)

---

## 7. Usage Examples

### 7.1 Basic Setup

**Define Entities:**
```csharp
public class Product : EntityBase<int>, ISoftDeletable
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}

public class Category : EntityBase<Guid>
{
    public string Name { get; set; }
}
```

**DbContext Configuration:**
```csharp
public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureSoftDelete(); // Enable soft delete filtering
    }
}
```

**Dependency Injection Setup:**
```csharp
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

services.AddScoped<IUnitOfWork, UnitOfWork<AppDbContext>>();
```

### 7.2 Using the Repository

**Controller/Service Example:**
```csharp
public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        var repo = _unitOfWork.Repository<Product, int>();
        await repo.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var repo = _unitOfWork.Repository<Product, int>();
        return await repo.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        var repo = _unitOfWork.Repository<Product, int>();
        return await repo.FindAsync(p => p.Name.Contains(searchTerm));
    }

    public async Task DeleteProductAsync(int id)
    {
        var repo = _unitOfWork.Repository<Product, int>();
        await repo.DeleteAsync(id); // Soft delete (sets IsDeleted = true)
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task HardDeleteProductAsync(int id)
    {
        var repo = _unitOfWork.Repository<Product, int>();
        var product = await repo.GetByIdAsync(id);
        if (product != null)
        {
            repo.HardDelete(product); // Permanent delete
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
```

### 7.3 Advanced Queries

```csharp
// With includes
var products = await repo
    .Include(p => p.Category)
    .Include(p => p.Reviews)
    .GetAllAsync();

// Pagination
var pagedProducts = await repo.GetPagedAsync(pageNumber: 1, pageSize: 20);

// No-tracking for read-only
var readOnlyProducts = await repo
    .AsNoTracking()
    .FindAsync(p => p.Price > 100);
```

### 7.4 Transaction Example

```csharp
public async Task TransferInventoryAsync(int productId, int fromWarehouse, int toWarehouse, int quantity)
{
    await using var transaction = await _unitOfWork.BeginTransactionAsync();
    try
    {
        var inventoryRepo = _unitOfWork.Repository<Inventory, int>();

        // Deduct from source
        var sourceInventory = await inventoryRepo.FindSingleAsync(
            i => i.ProductId == productId && i.WarehouseId == fromWarehouse);
        sourceInventory.Quantity -= quantity;
        inventoryRepo.Update(sourceInventory);

        // Add to destination
        var destInventory = await inventoryRepo.FindSingleAsync(
            i => i.ProductId == productId && i.WarehouseId == toWarehouse);
        destInventory.Quantity += quantity;
        inventoryRepo.Update(destInventory);

        await _unitOfWork.SaveChangesAsync();
        await _unitOfWork.CommitTransactionAsync();
    }
    catch
    {
        await _unitOfWork.RollbackTransactionAsync();
        throw;
    }
}
```

---

## 8. Testing Strategy

### Unit Tests
- Repository CRUD operations
- Soft delete behavior
- Query operations (Find, Any, Count, etc.)
- Include/AsNoTracking functionality
- Unit of Work coordination

### Integration Tests
- End-to-end workflows with in-memory database
- Transaction rollback scenarios
- Multiple repositories in single Unit of Work

### Test Projects
- `GenericRepository.Tests` - Unit tests using Moq/NSubstitute
- `GenericRepository.IntegrationTests` - Integration tests with EF Core InMemory provider

---

## 9. Open Questions & Decisions

### Q1: Framework Target
**Status**: ✓ DECIDED
**Decision**: .NET 6 + EF Core 8.0
**Rationale**: Modern features, LTS support, latest EF Core capabilities, good compatibility across .NET 6+

### Q2: Library Naming
**Status**: ✓ DECIDED
**Decision**: `EFCore.GenericRepository`
**Rationale**: Clear, descriptive, follows .NET conventions, excellent discoverability

### Q3: Specification Pattern
**Status**: ✓ DECIDED
**Decision**: Skip for v1.0
**Rationale**: Keeps the library simple and focused. Can be added in v2.0 if requested by users.

### Q4: Repository Registration
**Status**: ✓ DECIDED
**Decision**: Yes, include DI registration extension methods
**Implementation**: `services.AddGenericRepository<AppDbContext>()`
**Rationale**: Significantly improves developer experience with minimal effort.

### Q5: Query Result Types
**Status**: ✓ DECIDED
**Decision**: `IEnumerable<T>`
**Rationale**: Most flexible, standard practice, allows deferred execution and LINQ composition.

### Q6: Paging Result Type
**Status**: ✓ DECIDED
**Decision**: Rich pagination with `PagedResult<T>`
**Rationale**: Much more useful for building UIs with pagination controls. Provides total count and page metadata.
```csharp
public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}
```

### Q7: Sorting Support
**Status**: ✓ DECIDED
**Decision**: Yes, include sorting methods
**Rationale**: Very common requirement, significantly increases library usefulness.
```csharp
IRepositoryQueryBase<T, K, TContext> OrderBy(Expression<Func<T, object>> keySelector);
IRepositoryQueryBase<T, K, TContext> OrderByDescending(Expression<Func<T, object>> keySelector);
IRepositoryQueryBase<T, K, TContext> ThenBy(Expression<Func<T, object>> keySelector);
IRepositoryQueryBase<T, K, TContext> ThenByDescending(Expression<Func<T, object>> keySelector);
```

### Q8: Async-Only API
**Status**: ✓ DECIDED
**Decision**: Support both sync and async methods
**Rationale**: Broader compatibility, some legacy codebases still need synchronous methods.

---

## 10. Future Enhancements (v2+)

- Specification pattern support
- Bulk operations (BulkInsert, BulkUpdate, BulkDelete)
- Caching layer integration
- Auditing support (CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
- Query optimization hints
- Compiled queries for frequently-used operations
- GraphQL/OData query support
- Event publishing (domain events)
- Multi-tenancy support
- Change tracking and history

---

## 11. Versioning & Publishing Strategy

### 11.1 Semantic Versioning

We'll follow **Semantic Versioning 2.0.0** (semver.org):

**Format**: `MAJOR.MINOR.PATCH` (e.g., 1.2.3)

- **MAJOR**: Incompatible API changes (breaking changes)
- **MINOR**: New functionality (backwards-compatible)
- **PATCH**: Bug fixes (backwards-compatible)

**Pre-release versions**: `MAJOR.MINOR.PATCH-prerelease` (e.g., 1.0.0-beta.1, 2.0.0-rc.1)

**Version Examples**:
- `1.0.0` - Initial stable release
- `1.1.0` - Added new features (sorting, pagination metadata)
- `1.1.1` - Bug fixes
- `1.2.0` - Added new query methods
- `2.0.0` - Breaking changes (renamed interfaces, changed method signatures)
- `2.0.0-beta.1` - Beta release for v2

### 11.2 Git Workflow & Branching Strategy

**Branch Structure**:
- `main` - Stable releases only (protected)
- `develop` - Integration branch for next release
- `feature/*` - Feature branches (e.g., `feature/add-sorting`)
- `bugfix/*` - Bug fix branches (e.g., `bugfix/pagination-null-check`)
- `release/*` - Release preparation (e.g., `release/1.1.0`)
- `hotfix/*` - Critical production fixes (e.g., `hotfix/1.0.1`)

**Workflow**:
1. Create feature branch from `develop`
2. Implement and test feature
3. Create PR to `develop`
4. Code review and merge
5. When ready for release, create `release/x.x.x` branch
6. Test thoroughly, update version numbers, changelog
7. Merge to `main` and tag with version
8. Merge back to `develop`

### 11.3 Changelog Management

Use **Keep a Changelog** format (keepachangelog.com):

**File**: `CHANGELOG.md`

**Format**:
```markdown
# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Added
- Feature X
### Changed
- Behavior Y
### Fixed
- Bug Z

## [1.1.0] - 2024-12-15
### Added
- Sorting support with OrderBy/ThenBy methods
- Rich pagination with PagedResult<T>
- DI registration extension methods

### Changed
- Improved query performance with compiled queries

### Fixed
- Null reference exception in soft delete filter

## [1.0.0] - 2024-12-01
### Added
- Initial release
- Generic repository pattern
- Unit of Work implementation
- Soft delete support
```

### 11.4 NuGet Publishing Process

**Prerequisites**:
1. NuGet.org account
2. API key from nuget.org
3. Project configured with package metadata

**Publishing Methods**:

#### Option A: Manual Publishing
```bash
# Build the project in Release mode
dotnet build -c Release

# Pack the NuGet package
dotnet pack -c Release -o ./artifacts

# Push to NuGet.org
dotnet nuget push ./artifacts/EFCore.GenericRepository.1.0.0.nupkg \
  --api-key YOUR_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

#### Option B: Automated with GitHub Actions (Recommended)

**File**: `.github/workflows/publish-nuget.yml`

```yaml
name: Publish to NuGet

on:
  push:
    tags:
      - 'v*.*.*'  # Trigger on version tags (v1.0.0, v1.1.0, etc.)

jobs:
  publish:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3

      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '6.0.x'

      - name: Restore dependencies
        run: dotnet restore

      - name: Build
        run: dotnet build -c Release --no-restore

      - name: Test
        run: dotnet test -c Release --no-build --verbosity normal

      - name: Pack
        run: dotnet pack -c Release --no-build -o ./artifacts

      - name: Publish to NuGet
        run: dotnet nuget push ./artifacts/*.nupkg \
          --api-key ${{ secrets.NUGET_API_KEY }} \
          --source https://api.nuget.org/v3/index.json \
          --skip-duplicate
```

**Setup**:
1. Add NuGet API key to GitHub Secrets: `NUGET_API_KEY`
2. Create and push a version tag: `git tag v1.0.0 && git push origin v1.0.0`
3. GitHub Actions automatically builds, tests, and publishes

### 11.5 CI/CD Pipeline

**File**: `.github/workflows/ci.yml`

```yaml
name: CI Build

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]

jobs:
  build:
    runs-on: ubuntu-latest
    strategy:
      matrix:
        dotnet-version: ['6.0.x', '7.0.x', '8.0.x']

    steps:
      - uses: actions/checkout@v3

      - name: Setup .NET ${{ matrix.dotnet-version }}
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: ${{ matrix.dotnet-version }}

      - name: Restore dependencies
        run: dotnet restore

      - name: Build
        run: dotnet build --no-restore

      - name: Test
        run: dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage"

      - name: Upload coverage to Codecov
        uses: codecov/codecov-action@v3
        with:
          files: '**/coverage.cobertura.xml'
```

### 11.6 Release Process Checklist

**Pre-Release**:
- [ ] All features implemented and tested
- [ ] All unit tests passing
- [ ] All integration tests passing
- [ ] Code coverage ≥ 80%
- [ ] XML documentation complete
- [ ] README.md updated
- [ ] CHANGELOG.md updated
- [ ] Version number updated in .csproj
- [ ] Sample project tested

**Release**:
- [ ] Create release branch: `release/x.x.x`
- [ ] Final testing on release branch
- [ ] Merge release branch to `main`
- [ ] Tag release: `git tag vx.x.x`
- [ ] Push tag: `git push origin vx.x.x`
- [ ] CI/CD publishes to NuGet automatically
- [ ] Create GitHub Release with changelog
- [ ] Merge `main` back to `develop`

**Post-Release**:
- [ ] Verify package on NuGet.org
- [ ] Test installation: `dotnet add package EFCore.GenericRepository`
- [ ] Announce release (blog, social media, etc.)
- [ ] Monitor for issues

### 11.7 Version Planning

**v1.0.0** (Initial Release):
- Core repository pattern
- Unit of Work
- Soft delete
- Basic CRUD
- Query operations
- Sorting
- Rich pagination
- DI extensions

**v1.x.x** (Minor Updates):
- Performance improvements
- Bug fixes
- Additional query methods
- Documentation improvements

**v2.0.0** (Major Update - Future):
- Specification pattern
- Bulk operations
- Breaking API changes if needed
- Auditing support

### 11.8 Package Distribution

**Primary**: NuGet.org (public)
**Alternative**:
- GitHub Packages (backup)
- Azure Artifacts (for preview builds)
- MyGet (for CI builds)

**Package URLs**:
- NuGet: `https://www.nuget.org/packages/EFCore.GenericRepository`
- Installation: `dotnet add package EFCore.GenericRepository`

### 11.9 Support & Maintenance

**Support Channels**:
- GitHub Issues for bugs and feature requests
- GitHub Discussions for questions
- Documentation wiki

**Maintenance Schedule**:
- Security patches: Immediate
- Bug fixes: Within 1 week
- Feature requests: Next minor version
- Breaking changes: Next major version

**Deprecation Policy**:
- Deprecation warnings in CHANGELOG
- At least one minor version before removal
- Clear migration path documented

---

## 12. Documentation Requirements

### README.md
- Quick start guide
- Installation instructions
- Basic usage examples
- Advanced scenarios
- Migration guide
- FAQ

### XML Documentation
- All public APIs must have XML comments
- Include parameter descriptions
- Include example code in remarks
- Document exceptions

### Wiki/Docs Site (Optional)
- Architecture overview
- Design patterns used
- Best practices
- Performance considerations
- Troubleshooting guide

---

## 12. Release Checklist

- [ ] All unit tests passing
- [ ] All integration tests passing
- [ ] XML documentation complete
- [ ] README.md written
- [ ] NuGet package metadata configured
- [ ] Sample project created and tested
- [ ] License file added
- [ ] Changelog created
- [ ] Version number set
- [ ] Package built and tested locally
- [ ] Published to NuGet.org

---

## Next Steps

1. **Review this plan** - Read through and provide feedback
2. **Make decisions** - Answer open questions (Q1-Q8)
3. **Finalize scope** - Decide what goes in v1 vs future versions
4. **Begin implementation** - Start with Phase 1 once plan is approved

**Questions for you:**
1. Does this overall structure meet your needs?
2. Which framework target do you prefer (.NET 6 or .NET Standard 2.1)?
3. What should we name the library?
4. Any features you want to add or remove from the plan?
5. Should we include Specification pattern in v1 or defer to v2?
