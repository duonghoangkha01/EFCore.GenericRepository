using EFCore.GenericRepository.Tests.Entities;
using EFCore.GenericRepository.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EFCore.GenericRepository.Tests.Repositories
{
    /// <summary>
    /// Tests for soft delete query filtering functionality.
    /// </summary>
    public class SoftDeletableProductRepositoryTests : IDisposable
    {
        private readonly TestDbContext _dbContext;
        private readonly SoftDeletableProductRepository _sut;

        public SoftDeletableProductRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new TestDbContext(options);
            _sut = new SoftDeletableProductRepository(_dbContext);
        }

        private void SeedData()
        {
            _dbContext.Products.AddRange(
                new SoftDeletableProduct { Id = 1, Name = "Product 1", Price = 10.00m, IsDeleted = false },
                new SoftDeletableProduct { Id = 2, Name = "Product 2", Price = 20.00m, IsDeleted = false },
                new SoftDeletableProduct { Id = 3, Name = "Product 3", Price = 30.00m, IsDeleted = true, DeletedAt = DateTimeOffset.UtcNow }
            );
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task GetAll_ShouldExcludeSoftDeletedEntities()
        {
            // Arrange
            SeedData();

            // Act
            var products = await _sut.GetAllAsync();

            // Assert
            Assert.Equal(2, products.Count());
            Assert.All(products, p => Assert.False(p.IsDeleted));
        }

        [Fact]
        public async Task GetById_SoftDeletedEntity_ShouldReturnNull()
        {
            // Arrange
            SeedData();

            // Act
            var product = await _sut.GetByIdAsync(3); // ID 3 is soft deleted

            // Assert
            Assert.Null(product);
        }

        [Fact]
        public async Task Find_ShouldExcludeSoftDeletedEntities()
        {
            // Arrange
            SeedData();

            // Act
            var products = await _sut.FindAsync(p => p.Price >= 10.00m);

            // Assert
            Assert.Equal(2, products.Count());
            Assert.DoesNotContain(products, p => p.Id == 3);
        }

        [Fact]
        public async Task Count_ShouldExcludeSoftDeletedEntities()
        {
            // Arrange
            SeedData();

            // Act
            var count = await _sut.CountAsync();

            // Assert
            Assert.Equal(2, count);
        }

        [Fact]
        public async Task Any_SoftDeletedEntity_ShouldReturnFalse()
        {
            // Arrange
            SeedData();

            // Act
            var exists = await _sut.AnyAsync(p => p.Id == 3);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task Any_NonDeletedEntity_ShouldReturnTrue()
        {
            // Arrange
            SeedData();

            // Act
            var exists = await _sut.AnyAsync(p => p.Id == 1);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public void GetPaged_ShouldExcludeSoftDeletedEntities()
        {
            // Arrange
            SeedData();

            // Act
            var result = _sut.GetPaged(1, 10);

            // Assert
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.Items.Count());
            Assert.DoesNotContain(result.Items, p => p.IsDeleted);
        }

        [Fact]
        public void FindSingle_SoftDeletedEntity_ShouldReturnNull()
        {
            // Arrange
            SeedData();

            // Act
            var product = _sut.FindSingle(p => p.Id == 3);

            // Assert
            Assert.Null(product);
        }

        [Fact]
        public async Task Count_WithPredicate_ShouldExcludeSoftDeletedEntities()
        {
            // Arrange
            SeedData();

            // Act
            var count = await _sut.CountAsync(p => p.Price >= 10.00m);

            // Assert
            // Should count products 1 and 2, but not 3 (soft deleted)
            Assert.Equal(2, count);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
