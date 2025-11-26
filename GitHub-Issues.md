# EFCore.GenericRepository - GitHub Issues/Tasks

This file contains all tasks to be created as GitHub Issues.

## Milestone: v1.0.0-alpha

### Setup & Infrastructure

#### Issue #1: Create initial repository structure and .gitignore
**Labels**: `setup`, `priority: high`

**Description**:
Set up the initial repository structure with proper .NET solution and project organization.

**Tasks**:
- [x] Create solution file
- [x] Create main library project structure
- [x] Create test project structure
- [x] Add .gitignore for .NET
- [x] Set up directory structure as per plan

**Acceptance Criteria**:
- Solution builds successfully
- Proper folder structure in place
- .gitignore excludes build artifacts

---

#### Issue #2: Configure project properties and NuGet metadata
**Labels**: `setup`, `priority: high`

**Description**:
Configure the .csproj file with all necessary NuGet package metadata and dependencies.

**Tasks**:
- [x] Add package metadata (PackageId, Authors, Description, etc.)
- [x] Configure target framework (net6.0)
- [x] Add EF Core 8.0 dependencies
- [x] Enable nullable reference types
- [x] Enable XML documentation generation

**Acceptance Criteria**:
- Package metadata is complete
- All required dependencies added
- Project builds successfully

---

#### Issue #3: Create README.md with initial documentation
**Labels**: `documentation`, `priority: high`

**Description**:
Create initial README with project overview and placeholder content.

**Tasks**:
- [x] Add project overview and description
- [x] Add installation instructions (placeholder)
- [x] Add basic usage example
- [x] Add badges (build status, NuGet version, etc.)
- [x] Link to CONTRIBUTING.md

**Acceptance Criteria**:
- README is clear and informative
- Includes all essential sections

---

#### Issue #4: Set up CI/CD pipeline (GitHub Actions)
**Labels**: `ci-cd`, `priority: high`

**Description**:
Create GitHub Actions workflows for continuous integration.

**Tasks**:
- [x] Create `.github/workflows/ci.yml`
- [x] Configure build job
- [x] Configure test job with code coverage
- [x] Test on multiple .NET versions (6.0.x, 7.0.x, 8.0.x)
- [x] Configure triggers (push, PR)

**Acceptance Criteria**:
- CI runs on every push and PR
- All tests pass
- Code coverage is reported

---

### Core Entities & Interfaces

#### Issue #5: Implement EntityBase<K> class
**Labels**: `core`, `priority: high`

**Description**:
Create the base entity class with generic key type support.

**Tasks**:
- [x] Create `EntityBase<K>` abstract class
- [x] Add generic `Id` property
- [x] Add comprehensive XML documentation
- [x] Create unit tests

**Code**:
```csharp
namespace EFCore.GenericRepository.Entities;

/// <summary>
/// Base class for all entities with generic primary key type.
/// </summary>
/// <typeparam name="K">The type of the primary key.</typeparam>
public abstract class EntityBase<K>
{
    /// <summary>
    /// Gets or sets the primary key.
    /// </summary>
    public K Id { get; set; } = default!;
}
```

**Acceptance Criteria**:
- Class is abstract and generic
- XML documentation is complete
- Unit tests pass

---

#### Issue #6: Implement ISoftDeletable interface
**Labels**: `core`, `priority: high`

**Description**:
Create the interface for entities that support soft delete.

**Tasks**:
- [x] Create `ISoftDeletable` interface
- [x] Add `IsDeleted` property
- [x] Add `DeletedAt` property
- [x] Add comprehensive XML documentation

**Code**:
```csharp
namespace EFCore.GenericRepository.Entities;

/// <summary>
/// Marker interface for entities that support soft delete.
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// Gets or sets a value indicating whether this entity is soft deleted.
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the date and time when this entity was soft deleted.
    /// </summary>
    DateTime? DeletedAt { get; set; }
}
```

**Acceptance Criteria**:
- Interface is properly defined
- XML documentation is complete

---

#### Issue #7: Implement PagedResult<T> class
**Labels**: `core`, `priority: high`

**Description**:
Create the result type for paginated queries with metadata.

**Tasks**:
- [x] Create `PagedResult<T>` class
- [x] Add properties: Items, TotalCount, PageNumber, PageSize, TotalPages
- [x] Add calculated properties: HasPreviousPage, HasNextPage
- [x] Implement calculation logic for TotalPages
- [x] Add comprehensive XML documentation
- [x] Create unit tests

**Acceptance Criteria**:
- All properties implemented correctly
- Calculated properties work correctly
- XML documentation is complete
- Unit tests pass

---

### Repository Query Layer

#### Issue #8: Define IRepositoryQueryBase<T, K, TContext> interface
**Labels**: `repository`, `priority: high`

**Description**:
Define the interface for read-only repository operations.

**Tasks**:
- [x] Define all query methods (GetById, GetAll, Find, etc.)
- [x] Define Include() method for eager loading
- [x] Define AsNoTracking() method
- [x] Define sorting methods (OrderBy, OrderByDescending, ThenBy, ThenByDescending)
- [x] Define pagination method returning PagedResult<T>
- [x] Add comprehensive XML documentation for all methods

**Acceptance Criteria**:
- All method signatures defined
- Fluent interface support (returns IRepositoryQueryBase)
- XML documentation is complete

---

#### Issue #9: Implement RepositoryQueryBase<T, K, TContext> class
**Labels**: `repository`, `priority: high`

**Description**:
Implement the base class for read-only repository operations.

**Tasks**:
- [x] Create abstract class implementing IRepositoryQueryBase
- [x] Implement all query operations
- [x] Implement fluent interface (Include, OrderBy, AsNoTracking)
- [x] Add soft delete filtering (global query filter)
- [x] Implement pagination with metadata calculation
- [x] Add comprehensive XML documentation

**Acceptance Criteria**:
- All methods implemented correctly
- Fluent interface works
- Soft delete filtering works
- XML documentation is complete

---

#### Issue #10: Add unit tests for RepositoryQueryBase
**Labels**: `tests`, `priority: high`

**Description**:
Create comprehensive unit tests for RepositoryQueryBase.

**Tasks**:
- [x] Test GetById, GetAll, Find methods
- [x] Test Include for eager loading
- [x] Test AsNoTracking
- [x] Test sorting methods (OrderBy, ThenBy)
- [x] Test pagination
- [x] Test soft delete filtering
- [x] Test edge cases (null, empty results)

**Acceptance Criteria**:
- All tests pass
- Code coverage ≥ 80%
- Edge cases covered

---

### Repository CRUD Layer

#### Issue #11: Define IRepositoryBase<T, K, TContext> interface
**Labels**: `repository`, `priority: high`

**Description**:
Define the interface for full CRUD repository operations.

**Tasks**:
- [ ] Extend IRepositoryQueryBase
- [ ] Define Add methods (single and range)
- [ ] Define Update methods (single and range)
- [ ] Define Delete methods (soft delete by default)
- [ ] Define HardDelete methods (permanent deletion)
- [ ] Define Restore methods (for soft-deleted entities)
- [ ] Add comprehensive XML documentation

**Acceptance Criteria**:
- All method signatures defined
- XML documentation is complete

---

#### Issue #12: Implement RepositoryBase<T, K, TContext> class
**Labels**: `repository`, `priority: high`

**Description**:
Implement the complete repository with CRUD operations.

**Tasks**:
- [ ] Inherit from RepositoryQueryBase
- [ ] Implement IRepositoryBase
- [ ] Implement Add/AddRange
- [ ] Implement Update/UpdateRange
- [ ] Implement Delete (with soft delete logic for ISoftDeletable)
- [ ] Implement HardDelete (permanent deletion)
- [ ] Implement Restore (for ISoftDeletable)
- [ ] Add comprehensive XML documentation

**Acceptance Criteria**:
- All methods implemented correctly
- Soft delete logic works
- Hard delete works
- Restore works
- XML documentation is complete

---

#### Issue #13: Add unit tests for RepositoryBase CRUD operations
**Labels**: `tests`, `priority: high`

**Description**:
Create comprehensive unit tests for CRUD operations.

**Tasks**:
- [ ] Test Add/AddRange
- [ ] Test Update/UpdateRange
- [ ] Test Delete (soft delete for ISoftDeletable)
- [ ] Test Delete (hard delete for non-ISoftDeletable)
- [ ] Test HardDelete
- [ ] Test Restore
- [ ] Test edge cases

**Acceptance Criteria**:
- All tests pass
- Code coverage ≥ 80%
- Soft delete behavior verified

---

### Unit of Work

#### Issue #14: Define IUnitOfWork interface
**Labels**: `unit-of-work`, `priority: high`

**Description**:
Define the Unit of Work interface for transaction coordination.

**Tasks**:
- [ ] Define Repository<T, K>() method
- [ ] Define SaveChanges methods (sync and async)
- [ ] Define transaction methods (Begin, Commit, Rollback)
- [ ] Extend IDisposable
- [ ] Add comprehensive XML documentation

**Acceptance Criteria**:
- All method signatures defined
- XML documentation is complete

---

#### Issue #15: Implement UnitOfWork<TContext> class
**Labels**: `unit-of-work`, `priority: high`

**Description**:
Implement the Unit of Work pattern for coordinating repositories.

**Tasks**:
- [ ] Create generic UnitOfWork<TContext> class
- [ ] Implement repository caching (dictionary)
- [ ] Implement lazy repository creation
- [ ] Implement SaveChanges delegation to DbContext
- [ ] Implement transaction support
- [ ] Implement proper disposal pattern
- [ ] Add comprehensive XML documentation

**Acceptance Criteria**:
- Repository caching works
- SaveChanges delegates correctly
- Transactions work
- Disposal works correctly
- XML documentation is complete

---

#### Issue #16: Add unit tests for UnitOfWork
**Labels**: `tests`, `priority: high`

**Description**:
Create comprehensive unit tests for UnitOfWork.

**Tasks**:
- [ ] Test repository creation and caching
- [ ] Test SaveChanges
- [ ] Test transaction commit
- [ ] Test transaction rollback
- [ ] Test disposal
- [ ] Test multiple repositories in single UnitOfWork

**Acceptance Criteria**:
- All tests pass
- Code coverage ≥ 80%
- Repository caching verified

---

### Extensions

#### Issue #17: Implement DbContextExtensions for soft delete
**Labels**: `extensions`, `priority: high`

**Description**:
Create extension methods for configuring soft delete in DbContext.

**Tasks**:
- [ ] Create DbContextExtensions class
- [ ] Implement ConfigureSoftDelete method
- [ ] Apply global query filter for ISoftDeletable entities
- [ ] Add comprehensive XML documentation
- [ ] Create unit/integration tests

**Acceptance Criteria**:
- Global query filter works
- Soft-deleted entities are filtered automatically
- XML documentation is complete
- Tests pass

---

#### Issue #18: Implement ServiceCollectionExtensions for DI
**Labels**: `extensions`, `priority: high`

**Description**:
Create extension methods for dependency injection registration.

**Tasks**:
- [ ] Create ServiceCollectionExtensions class
- [ ] Implement AddGenericRepository<TContext>() method
- [ ] Add overload with DbContext configuration
- [ ] Add comprehensive XML documentation

**Acceptance Criteria**:
- DI registration works correctly
- Both overloads work
- XML documentation is complete

---

### Sample Project

#### Issue #19: Create sample console application
**Labels**: `sample`, `priority: medium`

**Description**:
Create a sample console app demonstrating library usage.

**Tasks**:
- [ ] Create console project
- [ ] Define sample entities (Product, Category)
- [ ] Create sample DbContext
- [ ] Demonstrate CRUD operations
- [ ] Demonstrate pagination and sorting
- [ ] Demonstrate soft delete and restore
- [ ] Add comprehensive comments

**Acceptance Criteria**:
- Sample runs successfully
- Demonstrates all key features
- Well-commented

---

#### Issue #20: Create sample ASP.NET Core Web API project
**Labels**: `sample`, `priority: low`

**Description**:
Create a sample Web API demonstrating library usage.

**Tasks**:
- [ ] Create Web API project
- [ ] Configure DI with AddGenericRepository
- [ ] Create CRUD endpoints
- [ ] Add Swagger documentation
- [ ] Add example requests/responses

**Acceptance Criteria**:
- API runs successfully
- Endpoints work correctly
- Swagger documentation complete

---

### Integration Tests

#### Issue #21: Create integration test project
**Labels**: `tests`, `priority: medium`

**Description**:
Set up integration test project with in-memory database.

**Tasks**:
- [ ] Create test project
- [ ] Configure in-memory database
- [ ] Create test fixtures
- [ ] Set up test data seeding

**Acceptance Criteria**:
- Test project builds
- In-memory database works
- Test infrastructure in place

---

#### Issue #22: Add integration tests for complete workflows
**Labels**: `tests`, `priority: medium`

**Description**:
Create end-to-end integration tests.

**Tasks**:
- [ ] Test complete CRUD workflow
- [ ] Test transaction scenarios (commit/rollback)
- [ ] Test multiple repositories in UnitOfWork
- [ ] Test soft delete end-to-end

**Acceptance Criteria**:
- All integration tests pass
- End-to-end workflows verified

---

## Milestone: v1.0.0-beta

### Documentation

#### Issue #23: Complete XML documentation for all public APIs
**Labels**: `documentation`, `priority: high`

**Description**:
Review and complete XML documentation for all public APIs.

**Tasks**:
- [ ] Review all XML comments
- [ ] Add code examples in remarks sections
- [ ] Document all parameters
- [ ] Document all exceptions
- [ ] Document return values

**Acceptance Criteria**:
- All public APIs documented
- Code examples included
- No documentation warnings

---

#### Issue #24: Create comprehensive README.md
**Labels**: `documentation`, `priority: high`

**Description**:
Create complete README with full documentation.

**Tasks**:
- [ ] Installation guide
- [ ] Quick start guide
- [ ] Full usage examples
- [ ] API reference overview
- [ ] FAQ section
- [ ] Contributing guidelines
- [ ] License information

**Acceptance Criteria**:
- README is comprehensive
- Examples are clear
- All sections complete

---

#### Issue #25: Create CONTRIBUTING.md
**Labels**: `documentation`, `priority: medium`

**Description**:
Create contribution guidelines.

**Tasks**:
- [ ] How to contribute
- [ ] Code style guidelines
- [ ] PR process
- [ ] Testing requirements
- [ ] Commit message format

**Acceptance Criteria**:
- Guidelines are clear
- Process is well-defined

---

#### Issue #26: Create CHANGELOG.md
**Labels**: `documentation`, `priority: high`

**Description**:
Set up changelog following Keep a Changelog format.

**Tasks**:
- [ ] Create CHANGELOG.md structure
- [ ] Document v1.0.0 features
- [ ] Add all changes from development

**Acceptance Criteria**:
- Follows Keep a Changelog format
- All changes documented

---

### Publishing

#### Issue #27: Set up NuGet publishing workflow
**Labels**: `ci-cd`, `publishing`, `priority: high`

**Description**:
Create GitHub Actions workflow for NuGet publishing.

**Tasks**:
- [ ] Create `.github/workflows/publish-nuget.yml`
- [ ] Configure trigger on version tags
- [ ] Configure NuGet API key secret
- [ ] Test package build
- [ ] Test package push (to test feed first)

**Acceptance Criteria**:
- Workflow triggers on tags
- Package builds successfully
- Publishing works

---

#### Issue #28: Create package icon and documentation images
**Labels**: `documentation`, `priority: low`

**Description**:
Create visual assets for the package.

**Tasks**:
- [ ] Design package icon (128x128)
- [ ] Create architecture diagram
- [ ] Create usage flow diagram
- [ ] Add diagrams to README

**Acceptance Criteria**:
- Icon is professional
- Diagrams are clear

---

## Milestone: v1.0.0

### Release

#### Issue #29: Final testing and bug fixes
**Labels**: `testing`, `priority: high`

**Description**:
Complete final testing pass before release.

**Tasks**:
- [ ] Run full test suite
- [ ] Fix any remaining bugs
- [ ] Performance testing
- [ ] Security review
- [ ] Test on different platforms

**Acceptance Criteria**:
- All tests pass
- No critical bugs
- Performance acceptable

---

#### Issue #30: Prepare v1.0.0 release
**Labels**: `release`, `priority: high`

**Description**:
Prepare for v1.0.0 release.

**Tasks**:
- [ ] Update version to 1.0.0 in .csproj
- [ ] Finalize CHANGELOG
- [ ] Create release branch
- [ ] Final review
- [ ] Tag release (v1.0.0)

**Acceptance Criteria**:
- Version numbers correct
- CHANGELOG complete
- Release tagged

---

#### Issue #31: Publish to NuGet.org
**Labels**: `release`, `publishing`, `priority: high`

**Description**:
Publish v1.0.0 to NuGet.org.

**Tasks**:
- [ ] Push v1.0.0 tag to trigger publish
- [ ] Verify automated publish succeeds
- [ ] Verify package on NuGet.org
- [ ] Test installation: `dotnet add package EFCore.GenericRepository`

**Acceptance Criteria**:
- Package published successfully
- Package installs correctly
- Package metadata correct

---

#### Issue #32: Create GitHub Release with notes
**Labels**: `release`, `priority: high`

**Description**:
Create GitHub Release for v1.0.0.

**Tasks**:
- [ ] Create GitHub Release from tag
- [ ] Write release notes
- [ ] Attach package artifacts
- [ ] Publish release
- [ ] Announce on social media/forums

**Acceptance Criteria**:
- Release published
- Release notes comprehensive
- Artifacts attached
