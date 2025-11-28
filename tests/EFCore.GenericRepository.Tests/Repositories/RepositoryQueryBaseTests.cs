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

        #region Count with Predicate Tests

        [Fact]
        public async Task CountAsync_WithPredicate_ShouldReturnCorrectCount()
        {
            // Arrange
            SeedData();

            // Act
            var count = await _sut.CountAsync(u => u.Id > 1);

            // Assert
            Assert.Equal(2, count);
        }

        [Fact]
        public void Count_WithPredicate_ShouldReturnCorrectCount()
        {
            // Arrange
            SeedData();

            // Act
            var count = _sut.Count(u => u.Id > 1);

            // Assert
            Assert.Equal(2, count);
        }

        #endregion

        #region Pagination Metadata Tests

        [Fact]
        public void GetPaged_ShouldCalculateTotalPages()
        {
            // Arrange
            SeedData();

            // Act
            var result = _sut.GetPaged(1, 2);

            // Assert
            Assert.Equal(2, result.TotalPages); // 3 items / 2 per page = 2 pages
        }

        [Fact]
        public void GetPaged_FirstPage_ShouldSetHasPreviousPageFalse()
        {
            // Arrange
            SeedData();

            // Act
            var result = _sut.GetPaged(1, 2);

            // Assert
            Assert.False(result.HasPreviousPage);
        }

        [Fact]
        public void GetPaged_SecondPage_ShouldSetHasPreviousPageTrue()
        {
            // Arrange
            SeedData();

            // Act
            var result = _sut.GetPaged(2, 2);

            // Assert
            Assert.True(result.HasPreviousPage);
        }

        [Fact]
        public void GetPaged_FirstPage_ShouldSetHasNextPageTrue()
        {
            // Arrange
            SeedData();

            // Act
            var result = _sut.GetPaged(1, 2);

            // Assert
            Assert.True(result.HasNextPage);
        }

        [Fact]
        public void GetPaged_LastPage_ShouldSetHasNextPageFalse()
        {
            // Arrange
            SeedData();

            // Act
            var result = _sut.GetPaged(2, 2);

            // Assert
            Assert.False(result.HasNextPage);
        }

        [Fact]
        public void GetPaged_WithDifferentPageSizes_ShouldReturnCorrectItems()
        {
            // Arrange
            SeedData();

            // Act
            var result1 = _sut.GetPaged(1, 1);
            var result2 = _sut.GetPaged(1, 3);

            // Assert
            Assert.Single(result1.Items);
            Assert.Equal(3, result2.Items.Count());
        }

        [Fact]
        public void GetPaged_WithEmptyResult_ShouldReturnEmptyPagedResult()
        {
            // Arrange - No data seeded

            // Act
            var result = _sut.GetPaged(1, 10);

            // Assert
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalCount);
            Assert.Equal(0, result.TotalPages);
        }

        [Fact]
        public void GetPaged_BeyondTotalPages_ShouldReturnEmptyItems()
        {
            // Arrange
            SeedData();

            // Act
            var result = _sut.GetPaged(10, 2);

            // Assert
            Assert.Empty(result.Items);
            Assert.Equal(3, result.TotalCount); // Still shows total count
        }

        #endregion

        #region Include Tests

        [Fact]
        public async Task Include_SingleNavigation_ShouldEagerLoadRelatedEntity()
        {
            // Arrange
            var role = new Role { Id = 1, Name = "Admin" };
            _dbContext.Roles.Add(role);
            _dbContext.Users.Add(new User { Id = 1, Name = "User 1", RoleId = 1 });
            _dbContext.SaveChanges();
            _dbContext.ChangeTracker.Clear();

            // Act
            var user = await _sut.Include(u => u.Role!).GetByIdAsync(1);

            // Assert
            Assert.NotNull(user);
            Assert.NotNull(user.Role);
            Assert.Equal("Admin", user.Role.Name);
        }

        [Fact]
        public async Task Include_MultipleNavigations_ShouldEagerLoadAllRelatedEntities()
        {
            // Arrange
            var role = new Role { Id = 1, Name = "Admin" };
            var user = new User { Id = 1, Name = "User 1", RoleId = 1 };
            _dbContext.Roles.Add(role);
            _dbContext.Users.Add(user);
            _dbContext.Posts.Add(new Post { Id = 1, Title = "Post 1", UserId = 1 });
            _dbContext.SaveChanges();
            _dbContext.ChangeTracker.Clear();

            // Act
            var result = await _sut.Include(u => u.Role!).Include(u => u.Posts).GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Role);
            Assert.Equal("Admin", result.Role.Name);
            Assert.NotEmpty(result.Posts);
            Assert.Single(result.Posts);
        }

        [Fact]
        public void Include_ThenExecuteQuery_ShouldReturnWithLoadedNavigations()
        {
            // Arrange
            var role = new Role { Id = 1, Name = "Admin" };
            _dbContext.Roles.Add(role);
            _dbContext.Users.Add(new User { Id = 1, Name = "User 1", RoleId = 1 });
            _dbContext.SaveChanges();
            _dbContext.ChangeTracker.Clear();

            // Act
            var users = _sut.Include(u => u.Role!).GetAll();

            // Assert
            var user = users.First();
            Assert.NotNull(user.Role);
            Assert.Equal("Admin", user.Role.Name);
        }

        #endregion

        #region AsNoTracking Tests

        [Fact]
        public void AsNoTracking_ShouldDisableChangeTracking()
        {
            // Arrange
            SeedData();
            _dbContext.ChangeTracker.Clear(); // Clear tracked entities from seeding

            // Act
            _sut.AsNoTracking().GetAll().ToList();

            // Assert
            Assert.Empty(_dbContext.ChangeTracker.Entries());
        }

        [Fact]
        public void AsNoTracking_WithFluentChaining_ShouldChainCorrectly()
        {
            // Arrange
            SeedData();
            _dbContext.ChangeTracker.Clear(); // Clear tracked entities from seeding

            // Act
            var result = _sut.AsNoTracking().OrderBy(u => u.Name).GetAll();

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Empty(_dbContext.ChangeTracker.Entries());
        }

        [Fact]
        public async Task AsNoTracking_RetrievedEntities_ShouldNotBeTracked()
        {
            // Arrange
            SeedData();
            _dbContext.ChangeTracker.Clear(); // Clear tracked entities from seeding

            // Act
            var user = await _sut.AsNoTracking().GetByIdAsync(1);
            user!.Name = "Modified";

            // Assert
            Assert.Empty(_dbContext.ChangeTracker.Entries());
        }

        #endregion

        #region Sorting Tests

        [Fact]
        public void OrderBy_ShouldSortAscending()
        {
            // Arrange
            _dbContext.Users.AddRange(new List<User>
            {
                new User { Id = 1, Name = "Charlie" },
                new User { Id = 2, Name = "Alice" },
                new User { Id = 3, Name = "Bob" }
            });
            _dbContext.SaveChanges();

            // Act
            var users = _sut.OrderBy(u => u.Name).GetAll().ToList();

            // Assert
            Assert.Equal("Alice", users[0].Name);
            Assert.Equal("Bob", users[1].Name);
            Assert.Equal("Charlie", users[2].Name);
        }

        [Fact]
        public void OrderByDescending_ShouldSortDescending()
        {
            // Arrange
            _dbContext.Users.AddRange(new List<User>
            {
                new User { Id = 1, Name = "Charlie" },
                new User { Id = 2, Name = "Alice" },
                new User { Id = 3, Name = "Bob" }
            });
            _dbContext.SaveChanges();

            // Act
            var users = _sut.OrderByDescending(u => u.Name).GetAll().ToList();

            // Assert
            Assert.Equal("Charlie", users[0].Name);
            Assert.Equal("Bob", users[1].Name);
            Assert.Equal("Alice", users[2].Name);
        }

        [Fact]
        public void OrderBy_ThenBy_ShouldSortByMultipleKeys()
        {
            // Arrange
            var role1 = new Role { Id = 1, Name = "Admin" };
            var role2 = new Role { Id = 2, Name = "User" };
            _dbContext.Roles.AddRange(role1, role2);
            _dbContext.Users.AddRange(new List<User>
            {
                new User { Id = 1, Name = "Charlie", RoleId = 1 },
                new User { Id = 2, Name = "Alice", RoleId = 1 },
                new User { Id = 3, Name = "Bob", RoleId = 2 },
                new User { Id = 4, Name = "David", RoleId = 2 }
            });
            _dbContext.SaveChanges();

            // Act
            var users = _sut.OrderBy(u => u.RoleId).ThenBy(u => u.Name).GetAll().ToList();

            // Assert
            Assert.Equal("Alice", users[0].Name);
            Assert.Equal("Charlie", users[1].Name);
            Assert.Equal("Bob", users[2].Name);
            Assert.Equal("David", users[3].Name);
        }

        [Fact]
        public void OrderByDescending_ThenByDescending_ShouldSortByMultipleKeysDescending()
        {
            // Arrange
            var role1 = new Role { Id = 1, Name = "Admin" };
            var role2 = new Role { Id = 2, Name = "User" };
            _dbContext.Roles.AddRange(role1, role2);
            _dbContext.Users.AddRange(new List<User>
            {
                new User { Id = 1, Name = "Charlie", RoleId = 1 },
                new User { Id = 2, Name = "Alice", RoleId = 1 },
                new User { Id = 3, Name = "Bob", RoleId = 2 },
                new User { Id = 4, Name = "David", RoleId = 2 }
            });
            _dbContext.SaveChanges();

            // Act
            var users = _sut.OrderByDescending(u => u.RoleId).ThenByDescending(u => u.Name).GetAll().ToList();

            // Assert
            Assert.Equal("David", users[0].Name);
            Assert.Equal("Bob", users[1].Name);
            Assert.Equal("Charlie", users[2].Name);
            Assert.Equal("Alice", users[3].Name);
        }

        [Fact]
        public void ThenBy_WithoutOrderBy_ShouldThrowException()
        {
            // Arrange
            SeedData();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _sut.ThenBy(u => u.Name).GetAll().ToList());
            Assert.Contains("ThenBy can only be applied to a sorted query", exception.Message);
        }

        [Fact]
        public void ThenByDescending_WithoutOrderBy_ShouldThrowException()
        {
            // Arrange
            SeedData();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _sut.ThenByDescending(u => u.Name).GetAll().ToList());
            Assert.Contains("ThenByDescending can only be applied to a sorted query", exception.Message);
        }

        #endregion

        #region Edge Case Tests

        [Fact]
        public async Task GetById_NonExistentId_ShouldReturnNull()
        {
            // Arrange
            SeedData();

            // Act
            var user = await _sut.GetByIdAsync(999);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public void Find_NoMatches_ShouldReturnEmptyCollection()
        {
            // Arrange
            SeedData();

            // Act
            var users = _sut.Find(u => u.Id > 100);

            // Assert
            Assert.Empty(users);
        }

        [Fact]
        public async Task FindSingle_NoMatches_ShouldReturnNull()
        {
            // Arrange
            SeedData();

            // Act
            var user = await _sut.FindSingleAsync(u => u.Id == 999);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public void FindSingle_MultipleMatches_ShouldThrowException()
        {
            // Arrange
            SeedData();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                _sut.FindSingle(u => u.Id > 0)
            );
        }

        [Fact]
        public async Task Any_NoMatches_ShouldReturnFalse()
        {
            // Arrange
            SeedData();

            // Act
            var exists = await _sut.AnyAsync(u => u.Id > 100);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public void Count_EmptyCollection_ShouldReturnZero()
        {
            // Arrange - No data seeded

            // Act
            var count = _sut.Count();

            // Assert
            Assert.Equal(0, count);
        }

        [Fact]
        public void FluentChaining_IncludeOrderByAsNoTracking_ShouldWorkCorrectly()
        {
            // Arrange
            var role = new Role { Id = 1, Name = "Admin" };
            _dbContext.Roles.Add(role);
            _dbContext.Users.AddRange(new List<User>
            {
                new User { Id = 1, Name = "Charlie", RoleId = 1 },
                new User { Id = 2, Name = "Alice", RoleId = 1 },
                new User { Id = 3, Name = "Bob", RoleId = 1 }
            });
            _dbContext.SaveChanges();
            _dbContext.ChangeTracker.Clear();

            // Act
            var users = _sut
                .Include(u => u.Role!)
                .OrderBy(u => u.Name)
                .AsNoTracking()
                .GetAll()
                .ToList();

            // Assert
            Assert.Equal(3, users.Count);
            Assert.Equal("Alice", users[0].Name);
            Assert.Equal("Bob", users[1].Name);
            Assert.Equal("Charlie", users[2].Name);
            Assert.NotNull(users[0].Role);
            Assert.Equal("Admin", users[0].Role!.Name);
            Assert.Empty(_dbContext.ChangeTracker.Entries());
        }

        #endregion

        
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
