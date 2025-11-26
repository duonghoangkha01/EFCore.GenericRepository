using EFCore.GenericRepository.Tests.Entities;
using EFCore.GenericRepository.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EFCore.GenericRepository.Tests.Repositories
{
    public class RepositoryBaseTests
    {
        private TestDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new TestDbContext(options);
        }

        #region Add Operations Tests

        [Fact]
        public async Task AddAsync_ShouldAddEntity()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new PostRepository(context);
            var post = new Post { Title = "Test Post", Content = "Test Content", UserId = 1 };

            // Act
            var result = await repository.AddAsync(post);
            await context.SaveChangesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(post.Title, result.Title);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public void Add_ShouldAddEntity()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new PostRepository(context);
            var post = new Post { Title = "Test Post", Content = "Test Content", UserId = 1 };

            // Act
            var result = repository.Add(post);
            context.SaveChanges();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(post.Title, result.Title);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task AddRangeAsync_ShouldAddMultipleEntities()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new PostRepository(context);
            var posts = new List<Post>
            {
                new Post { Title = "Post 1", Content = "Content 1", UserId = 1 },
                new Post { Title = "Post 2", Content = "Content 2", UserId = 1 },
                new Post { Title = "Post 3", Content = "Content 3", UserId = 1 }
            };

            // Act
            await repository.AddRangeAsync(posts);
            await context.SaveChangesAsync();

            // Assert
            var savedPosts = await context.Posts.ToListAsync();
            Assert.Equal(3, savedPosts.Count);
        }

        [Fact]
        public void AddRange_ShouldAddMultipleEntities()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new PostRepository(context);
            var posts = new List<Post>
            {
                new Post { Title = "Post 1", Content = "Content 1", UserId = 1 },
                new Post { Title = "Post 2", Content = "Content 2", UserId = 1 }
            };

            // Act
            repository.AddRange(posts);
            context.SaveChanges();

            // Assert
            var savedPosts = context.Posts.ToList();
            Assert.Equal(2, savedPosts.Count);
        }

        #endregion

        #region Update Operations Tests

        [Fact]
        public void Update_ShouldUpdateEntity()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new PostRepository(context);
            var post = new Post { Title = "Original Title", Content = "Original Content", UserId = 1 };
            context.Posts.Add(post);
            context.SaveChanges();

            // Act
            post.Title = "Updated Title";
            var result = repository.Update(post);
            context.SaveChanges();

            // Assert
            var updatedPost = context.Posts.Find(post.Id);
            Assert.NotNull(updatedPost);
            Assert.Equal("Updated Title", updatedPost.Title);
        }

        [Fact]
        public void UpdateRange_ShouldUpdateMultipleEntities()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new PostRepository(context);
            var posts = new List<Post>
            {
                new Post { Title = "Post 1", Content = "Content 1", UserId = 1 },
                new Post { Title = "Post 2", Content = "Content 2", UserId = 1 }
            };
            context.Posts.AddRange(posts);
            context.SaveChanges();

            // Act
            posts[0].Title = "Updated Post 1";
            posts[1].Title = "Updated Post 2";
            repository.UpdateRange(posts);
            context.SaveChanges();

            // Assert
            var updatedPosts = context.Posts.ToList();
            Assert.Equal("Updated Post 1", updatedPosts[0].Title);
            Assert.Equal("Updated Post 2", updatedPosts[1].Title);
        }

        #endregion

        #region Delete Operations Tests (Soft Delete)

        [Fact]
        public async Task DeleteAsync_WithSoftDeletableEntity_ShouldSoftDelete()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new SoftDeletableProductRepository(context);
            var product = new SoftDeletableProduct { Name = "Test Product", Price = 10.99m };
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            // Act
            await repository.DeleteAsync(product.Id);
            await context.SaveChangesAsync();

            // Assert
            var deletedProduct = await context.Products.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == product.Id);
            Assert.NotNull(deletedProduct);
            Assert.True(deletedProduct.IsDeleted);
            Assert.NotNull(deletedProduct.DeletedAt);

            // Verify soft-deleted entity is not returned by normal queries
            var normalQuery = await context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            Assert.Null(normalQuery);
        }

        [Fact]
        public void Delete_WithSoftDeletableEntity_ShouldSoftDelete()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new SoftDeletableProductRepository(context);
            var product = new SoftDeletableProduct { Name = "Test Product", Price = 10.99m };
            context.Products.Add(product);
            context.SaveChanges();

            // Act
            repository.Delete(product.Id);
            context.SaveChanges();

            // Assert
            var deletedProduct = context.Products.IgnoreQueryFilters().FirstOrDefault(p => p.Id == product.Id);
            Assert.NotNull(deletedProduct);
            Assert.True(deletedProduct.IsDeleted);
            Assert.NotNull(deletedProduct.DeletedAt);
        }

        [Fact]
        public void Delete_WithSoftDeletableEntity_ByEntity_ShouldSoftDelete()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new SoftDeletableProductRepository(context);
            var product = new SoftDeletableProduct { Name = "Test Product", Price = 10.99m };
            context.Products.Add(product);
            context.SaveChanges();

            // Act
            repository.Delete(product);
            context.SaveChanges();

            // Assert
            var deletedProduct = context.Products.IgnoreQueryFilters().FirstOrDefault(p => p.Id == product.Id);
            Assert.NotNull(deletedProduct);
            Assert.True(deletedProduct.IsDeleted);
        }

        [Fact]
        public async Task DeleteRangeAsync_WithSoftDeletableEntities_ShouldSoftDeleteAll()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new SoftDeletableProductRepository(context);
            var products = new List<SoftDeletableProduct>
            {
                new SoftDeletableProduct { Name = "Product 1", Price = 10.99m },
                new SoftDeletableProduct { Name = "Product 2", Price = 20.99m }
            };
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
            var productIds = products.Select(p => p.Id).ToList();

            // Act
            await repository.DeleteRangeAsync(productIds);
            await context.SaveChangesAsync();

            // Assert
            var deletedProducts = await context.Products.IgnoreQueryFilters().Where(p => productIds.Contains(p.Id)).ToListAsync();
            Assert.All(deletedProducts, p => Assert.True(p.IsDeleted));
        }

        [Fact]
        public void DeleteRange_WithSoftDeletableEntities_ShouldSoftDeleteAll()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new SoftDeletableProductRepository(context);
            var products = new List<SoftDeletableProduct>
            {
                new SoftDeletableProduct { Name = "Product 1", Price = 10.99m },
                new SoftDeletableProduct { Name = "Product 2", Price = 20.99m }
            };
            context.Products.AddRange(products);
            context.SaveChanges();

            // Act
            repository.DeleteRange(products);
            context.SaveChanges();

            // Assert
            var deletedProducts = context.Products.IgnoreQueryFilters().Where(p => products.Select(pr => pr.Id).Contains(p.Id)).ToList();
            Assert.All(deletedProducts, p => Assert.True(p.IsDeleted));
        }

        #endregion

        #region Delete Operations Tests (Hard Delete for Non-Soft-Deletable)

        [Fact]
        public async Task DeleteAsync_WithRegularEntity_ShouldHardDelete()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new PostRepository(context);
            var post = new Post { Title = "Test Post", Content = "Test Content", UserId = 1 };
            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();

            // Act
            await repository.DeleteAsync(post.Id);
            await context.SaveChangesAsync();

            // Assert
            var deletedPost = await context.Posts.FindAsync(post.Id);
            Assert.Null(deletedPost);
        }

        [Fact]
        public void Delete_WithRegularEntity_ShouldHardDelete()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new PostRepository(context);
            var post = new Post { Title = "Test Post", Content = "Test Content", UserId = 1 };
            context.Posts.Add(post);
            context.SaveChanges();

            // Act
            repository.Delete(post);
            context.SaveChanges();

            // Assert
            var deletedPost = context.Posts.Find(post.Id);
            Assert.Null(deletedPost);
        }

        [Fact]
        public void DeleteRange_WithRegularEntities_ShouldHardDeleteAll()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new PostRepository(context);
            var posts = new List<Post>
            {
                new Post { Title = "Post 1", Content = "Content 1", UserId = 1 },
                new Post { Title = "Post 2", Content = "Content 2", UserId = 1 }
            };
            context.Posts.AddRange(posts);
            context.SaveChanges();

            // Act
            repository.DeleteRange(posts);
            context.SaveChanges();

            // Assert
            var remainingPosts = context.Posts.ToList();
            Assert.Empty(remainingPosts);
        }

        #endregion

        #region HardDelete Operations Tests

        [Fact]
        public void HardDelete_WithSoftDeletableEntity_ShouldPermanentlyDelete()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new SoftDeletableProductRepository(context);
            var product = new SoftDeletableProduct { Name = "Test Product", Price = 10.99m };
            context.Products.Add(product);
            context.SaveChanges();

            // Act
            repository.HardDelete(product);
            context.SaveChanges();

            // Assert
            var deletedProduct = context.Products.IgnoreQueryFilters().FirstOrDefault(p => p.Id == product.Id);
            Assert.Null(deletedProduct); // Should be completely removed
        }

        [Fact]
        public void HardDeleteRange_WithSoftDeletableEntities_ShouldPermanentlyDeleteAll()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new SoftDeletableProductRepository(context);
            var products = new List<SoftDeletableProduct>
            {
                new SoftDeletableProduct { Name = "Product 1", Price = 10.99m },
                new SoftDeletableProduct { Name = "Product 2", Price = 20.99m }
            };
            context.Products.AddRange(products);
            context.SaveChanges();
            var productIds = products.Select(p => p.Id).ToList();

            // Act
            repository.HardDeleteRange(products);
            context.SaveChanges();

            // Assert
            var deletedProducts = context.Products.IgnoreQueryFilters().Where(p => productIds.Contains(p.Id)).ToList();
            Assert.Empty(deletedProducts); // All should be completely removed
        }

        #endregion

        #region Restore Operations Tests

        [Fact]
        public async Task RestoreAsync_WithSoftDeletedEntity_ShouldRestoreEntity()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new SoftDeletableProductRepository(context);
            var product = new SoftDeletableProduct { Name = "Test Product", Price = 10.99m };
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            // Soft delete the product
            await repository.DeleteAsync(product.Id);
            await context.SaveChangesAsync();

            // Act
            await repository.RestoreAsync(product.Id);
            await context.SaveChangesAsync();

            // Assert
            var restoredProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            Assert.NotNull(restoredProduct);
            Assert.False(restoredProduct.IsDeleted);
            Assert.Null(restoredProduct.DeletedAt);
        }

        [Fact]
        public void Restore_WithSoftDeletedEntity_ShouldRestoreEntity()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new SoftDeletableProductRepository(context);
            var product = new SoftDeletableProduct { Name = "Test Product", Price = 10.99m };
            context.Products.Add(product);
            context.SaveChanges();

            // Soft delete the product
            repository.Delete(product.Id);
            context.SaveChanges();

            // Act
            repository.Restore(product.Id);
            context.SaveChanges();

            // Assert
            var restoredProduct = context.Products.FirstOrDefault(p => p.Id == product.Id);
            Assert.NotNull(restoredProduct);
            Assert.False(restoredProduct.IsDeleted);
            Assert.Null(restoredProduct.DeletedAt);
        }

        [Fact]
        public void Restore_WithEntity_ShouldRestoreEntity()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new SoftDeletableProductRepository(context);
            var product = new SoftDeletableProduct { Name = "Test Product", Price = 10.99m };
            context.Products.Add(product);
            context.SaveChanges();

            // Soft delete the product
            repository.Delete(product);
            context.SaveChanges();

            // Get the soft-deleted entity
            var deletedProduct = context.Products.IgnoreQueryFilters().First(p => p.Id == product.Id);

            // Act
            repository.Restore(deletedProduct);
            context.SaveChanges();

            // Assert
            var restoredProduct = context.Products.FirstOrDefault(p => p.Id == product.Id);
            Assert.NotNull(restoredProduct);
            Assert.False(restoredProduct.IsDeleted);
        }

        [Fact]
        public void Restore_WithRegularEntity_ShouldDoNothing()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new PostRepository(context);
            var post = new Post { Title = "Test Post", Content = "Test Content", UserId = 1 };
            context.Posts.Add(post);
            context.SaveChanges();

            // Act - Restore should do nothing for non-soft-deletable entities
            repository.Restore(post);
            context.SaveChanges();

            // Assert - Entity should still exist unchanged
            var unchangedPost = context.Posts.Find(post.Id);
            Assert.NotNull(unchangedPost);
        }

        #endregion

        #region Edge Cases Tests

        [Fact]
        public async Task DeleteAsync_WithNonExistentId_ShouldNotThrow()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new PostRepository(context);

            // Act & Assert - Should not throw
            await repository.DeleteAsync(9999);
            await context.SaveChangesAsync();
        }

        [Fact]
        public void Delete_WithNonExistentId_ShouldNotThrow()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new PostRepository(context);

            // Act & Assert - Should not throw
            repository.Delete(9999);
            context.SaveChanges();
        }

        [Fact]
        public async Task RestoreAsync_WithNonExistentId_ShouldNotThrow()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new SoftDeletableProductRepository(context);

            // Act & Assert - Should not throw
            await repository.RestoreAsync(9999);
            await context.SaveChangesAsync();
        }

        [Fact]
        public void Restore_WithNonExistentId_ShouldNotThrow()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new SoftDeletableProductRepository(context);

            // Act & Assert - Should not throw
            repository.Restore(9999);
            context.SaveChanges();
        }

        [Fact]
        public void AddRange_WithEmptyList_ShouldNotThrow()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new PostRepository(context);
            var emptyList = new List<Post>();

            // Act & Assert - Should not throw
            repository.AddRange(emptyList);
            context.SaveChanges();
        }

        [Fact]
        public void DeleteRange_WithEmptyList_ShouldNotThrow()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new PostRepository(context);
            var emptyList = new List<Post>();

            // Act & Assert - Should not throw
            repository.DeleteRange(emptyList);
            context.SaveChanges();
        }

        #endregion
    }
}
