using EFCore.GenericRepository.Extensions;
using EFCore.GenericRepository.Tests.Entities;
using EFCore.GenericRepository.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EFCore.GenericRepository.Tests.Extensions
{
    /// <summary>
    /// Unit tests for DbContextExtensions.
    /// </summary>
    public class DbContextExtensionsTests : IDisposable
    {
        private readonly TestDbContext _context;

        public DbContextExtensionsTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        #region ConfigureSoftDelete Tests

        [Fact]
        public async Task ConfigureSoftDelete_ShouldFilterSoftDeletedEntities_Automatically()
        {
            // Arrange
            var product1 = new SoftDeletableProduct { Name = "Active Product", Price = 10.99m };
            var product2 = new SoftDeletableProduct { Name = "Deleted Product", Price = 20.99m, IsDeleted = true, DeletedAt = DateTimeOffset.UtcNow };

            await _context.Products.AddRangeAsync(product1, product2);
            await _context.SaveChangesAsync();

            // Act
            var products = await _context.Products.ToListAsync();

            // Assert
            Assert.Single(products);
            Assert.Equal("Active Product", products[0].Name);
        }

        [Fact]
        public async Task ConfigureSoftDelete_ShouldIncludeSoftDeletedEntities_WithIgnoreQueryFilters()
        {
            // Arrange
            var product1 = new SoftDeletableProduct { Name = "Active Product", Price = 10.99m };
            var product2 = new SoftDeletableProduct { Name = "Deleted Product", Price = 20.99m, IsDeleted = true, DeletedAt = DateTimeOffset.UtcNow };

            await _context.Products.AddRangeAsync(product1, product2);
            await _context.SaveChangesAsync();

            // Act
            var allProducts = await _context.Products.IgnoreQueryFilters().ToListAsync();

            // Assert
            Assert.Equal(2, allProducts.Count);
            Assert.Contains(allProducts, p => p.Name == "Active Product");
            Assert.Contains(allProducts, p => p.Name == "Deleted Product");
        }

        [Fact]
        public async Task ConfigureSoftDelete_ShouldNotAffectNonSoftDeletableEntities()
        {
            // Arrange
            var post1 = new Post { Title = "Post 1", Content = "Content 1" };
            var post2 = new Post { Title = "Post 2", Content = "Content 2" };

            await _context.Posts.AddRangeAsync(post1, post2);
            await _context.SaveChangesAsync();

            // Act
            var posts = await _context.Posts.ToListAsync();

            // Assert - Both posts should be returned since Post doesn't implement ISoftDeletable
            Assert.Equal(2, posts.Count);
        }

        [Fact]
        public async Task ConfigureSoftDelete_ShouldFilterByIsDeletedFalse()
        {
            // Arrange - Create mix of active and deleted products
            var activeProducts = Enumerable.Range(1, 5).Select(i => new SoftDeletableProduct
            {
                Name = $"Active Product {i}",
                Price = i * 10m,
                IsDeleted = false
            }).ToList();

            var deletedProducts = Enumerable.Range(1, 3).Select(i => new SoftDeletableProduct
            {
                Name = $"Deleted Product {i}",
                Price = i * 15m,
                IsDeleted = true,
                DeletedAt = DateTimeOffset.UtcNow
            }).ToList();

            await _context.Products.AddRangeAsync(activeProducts);
            await _context.Products.AddRangeAsync(deletedProducts);
            await _context.SaveChangesAsync();

            // Act
            var visibleProducts = await _context.Products.ToListAsync();

            // Assert
            Assert.Equal(5, visibleProducts.Count);
            Assert.All(visibleProducts, p => Assert.False(p.IsDeleted));
        }

        [Fact]
        public async Task ConfigureSoftDelete_ShouldWorkWithFind()
        {
            // Arrange
            var activeProduct = new SoftDeletableProduct { Name = "Active", Price = 10m };
            var deletedProduct = new SoftDeletableProduct { Name = "Deleted", Price = 20m, IsDeleted = true, DeletedAt = DateTimeOffset.UtcNow };

            await _context.Products.AddRangeAsync(activeProduct, deletedProduct);
            await _context.SaveChangesAsync();

            // Act
            var foundActive = await _context.Products.FindAsync(activeProduct.Id);
            var foundDeleted = await _context.Products.FindAsync(deletedProduct.Id);

            // Assert
            // Note: Find() bypasses query filters, so both should be found
            // This is expected EF Core behavior
            Assert.NotNull(foundActive);
            Assert.NotNull(foundDeleted);
        }

        [Fact]
        public async Task ConfigureSoftDelete_ShouldWorkWithWhereClause()
        {
            // Arrange
            var products = new[]
            {
                new SoftDeletableProduct { Name = "Expensive Active", Price = 100m },
                new SoftDeletableProduct { Name = "Cheap Active", Price = 10m },
                new SoftDeletableProduct { Name = "Expensive Deleted", Price = 100m, IsDeleted = true, DeletedAt = DateTimeOffset.UtcNow }
            };

            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            // Act
            var expensiveProducts = await _context.Products
                .Where(p => p.Price > 50)
                .ToListAsync();

            // Assert - Should only return the active expensive product
            Assert.Single(expensiveProducts);
            Assert.Equal("Expensive Active", expensiveProducts[0].Name);
        }

        [Fact]
        public async Task ConfigureSoftDelete_ShouldWorkWithCount()
        {
            // Arrange
            var products = new[]
            {
                new SoftDeletableProduct { Name = "Product 1", Price = 10m },
                new SoftDeletableProduct { Name = "Product 2", Price = 20m },
                new SoftDeletableProduct { Name = "Product 3", Price = 30m, IsDeleted = true, DeletedAt = DateTimeOffset.UtcNow }
            };

            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            // Act
            var activeCount = await _context.Products.CountAsync();
            var totalCount = await _context.Products.IgnoreQueryFilters().CountAsync();

            // Assert
            Assert.Equal(2, activeCount);
            Assert.Equal(3, totalCount);
        }

        #endregion
    }
}
