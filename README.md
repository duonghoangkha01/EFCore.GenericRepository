# EFCore.GenericRepository

[![CI Build](https://github.com/duonghoangkha01/EFCore.GenericRepository/actions/workflows/ci.yml/badge.svg)](https://github.com/duonghoangkha01/EFCore.GenericRepository/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/EFCore.GenericRepository.svg)](https://www.nuget.org/packages/EFCore.GenericRepository/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Generic Repository pattern implementation with Unit of Work for Entity Framework Core 8.0. Provides reusable base repository with CRUD operations, soft delete support, and advanced query capabilities.

## ⚠️ Work in Progress

This project is currently under active development. The v1.0.0 release is planned for Q1 2025.

See [PLAN.md](PLAN.md) for detailed implementation plan and progress tracking.

## Features (Planned for v1.0.0)

- ✅ **Generic Repository Pattern** - Type-safe repository base classes
- ✅ **Unit of Work Pattern** - Coordinate multiple repositories
- ✅ **Soft Delete Support** - Automatic filtering of deleted entities
- ✅ **Rich Pagination** - Pagination with metadata (total count, page info)
- ✅ **Sorting Support** - Fluent OrderBy/ThenBy methods
- ✅ **Query Operations** - GetById, GetAll, Find, FindSingle, Any, Count
- ✅ **CRUD Operations** - Add, Update, Delete, Restore, HardDelete
- ✅ **Eager Loading** - Include support for navigation properties
- ✅ **No-Tracking Queries** - AsNoTracking for read-only scenarios
- ✅ **Async/Await** - Full async support with synchronous alternatives
- ✅ **Dependency Injection** - Easy DI registration
- ✅ **XML Documentation** - IntelliSense support for all APIs

## Installation

_Coming soon - Package will be available on NuGet when v1.0.0 is released._

```bash
dotnet add package EFCore.GenericRepository
```

## Quick Start

_Documentation will be completed with v1.0.0 release._

### Define Your Entities

```csharp
public class Product : EntityBase<int>, ISoftDeletable
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
```

### Configure DbContext

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureSoftDelete(); // Enable soft delete filtering
    }
}
```

### Register Services

```csharp
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

services.AddGenericRepository<AppDbContext>();
```

### Use in Your Services

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
}
```

## Documentation

- [Implementation Plan](PLAN.md) - Detailed project plan and progress
- [GitHub Issues](GitHub-Issues.md) - Task list and milestones
- [Contributing Guidelines](CONTRIBUTING.md)
- [Changelog](CHANGELOG.md)

## Requirements

- .NET 8.0 or higher
- Entity Framework Core 8.0

## Project Status

**Current Phase**: Initial Setup Complete
**Progress**: See [PLAN.md](PLAN.md) for detailed progress tracking

### Milestones
- [ ] v1.0.0-alpha - Core implementation
- [ ] v1.0.0-beta - Documentation and testing
- [ ] v1.0.0 - Production release

## Contributing

Contributions are welcome! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for details.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

Built with ❤️ using Entity Framework Core

---

**Note**: This project is under active development. APIs may change before v1.0.0 release.
