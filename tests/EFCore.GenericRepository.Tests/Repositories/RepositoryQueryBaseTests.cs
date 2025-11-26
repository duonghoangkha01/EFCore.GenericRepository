using EFCore.GenericRepository.Tests.Entities;
using EFCore.GenericRepository.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EFCore.GenericRepository.Tests.Repositories
{
    public class RepositoryQueryBaseTests : IDisposable
    {
        private readonly TestDbContext _dbContext;
        private readonly TestRepository _sut;

        public RepositoryQueryBaseTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new TestDbContext(options);
            _sut = new TestRepository(_dbContext);
        }

        private void SeedData()
        {
            _dbContext.Users.AddRange(new List<User>
            {
                new User { Id = 1, Name = "User 1" },
                new User { Id = 2, Name = "User 2" },
                new User { Id = 3, Name = "User 3" }
            });
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectEntity()
        {
            // Arrange
            SeedData();

            // Act
            var user = await _sut.GetByIdAsync(1);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(1, user.Id);
        }

        [Fact]
        public void GetById_ShouldReturnCorrectEntity()
        {
            // Arrange
            SeedData();

            // Act
            var user = _sut.GetById(1);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(1, user.Id);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            // Arrange
            SeedData();

            // Act
            var users = await _sut.GetAllAsync();

            // Assert
            Assert.Equal(3, users.Count());
        }

        [Fact]
        public void GetAll_ShouldReturnAllEntities()
        {
            // Arrange
            SeedData();

            // Act
            var users = _sut.GetAll();

            // Assert
            Assert.Equal(3, users.Count());
        }

        [Fact]
        public async Task FindAsync_ShouldReturnFilteredEntities()
        {
            // Arrange
            SeedData();

            // Act
            var users = await _sut.FindAsync(u => u.Id > 1);

            // Assert
            Assert.Equal(2, users.Count());
        }

        [Fact]
        public void Find_ShouldReturnFilteredEntities()
        {
            // Arrange
            SeedData();

            // Act
            var users = _sut.Find(u => u.Id > 1);

            // Assert
            Assert.Equal(2, users.Count());
        }

        [Fact]
        public async Task FindSingleAsync_ShouldReturnCorrectEntity()
        {
            // Arrange
            SeedData();

            // Act
            var user = await _sut.FindSingleAsync(u => u.Id == 2);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(2, user.Id);
        }

        [Fact]
        public void FindSingle_ShouldReturnCorrectEntity()
        {
            // Arrange
            SeedData();

            // Act
            var user = _sut.FindSingle(u => u.Id == 2);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(2, user.Id);
        }

        [Fact]
        public async Task AnyAsync_ShouldReturnTrue_WhenEntityExists()
        {
            // Arrange
            SeedData();

            // Act
            var exists = await _sut.AnyAsync(u => u.Id == 1);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public void Any_ShouldReturnTrue_WhenEntityExists()
        {
            // Arrange
            SeedData();

            // Act
            var exists = _sut.Any(u => u.Id == 1);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task CountAsync_ShouldReturnCorrectCount()
        {
            // Arrange
            SeedData();

            // Act
            var count = await _sut.CountAsync();

            // Assert
            Assert.Equal(3, count);
        }

        [Fact]
        public void Count_ShouldReturnCorrectCount()
        {
            // Arrange
            SeedData();

            // Act
            var count = _sut.Count();

            // Assert
            Assert.Equal(3, count);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldReturnPagedResult()
        {
            // Arrange
            SeedData();

            // Act
            var result = await _sut.GetPagedAsync(1, 2);

            // Assert
            Assert.Equal(2, result.Items.Count());
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(2, result.PageSize);
        }

        [Fact]
        public void GetPaged_ShouldReturnPagedResult()
        {
            // Arrange
            SeedData();

            // Act
            var result = _sut.GetPaged(1, 2);

            // Assert
            Assert.Equal(2, result.Items.Count());
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(2, result.PageSize);
        }
        
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
