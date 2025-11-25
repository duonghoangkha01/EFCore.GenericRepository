using EFCore.GenericRepository.Entities;
using Xunit;

namespace EFCore.GenericRepository.Tests.Entities
{
    public class EntityBaseTests
    {
        private class TestEntity<T> : EntityBase<T> { }

        [Fact]
        public void EntityBase_WithIntKey_ShouldSetAndGetId()
        {
            // Arrange
            var entity = new TestEntity<int>();
            var id = 1;

            // Act
            entity.Id = id;

            // Assert
            Assert.Equal(id, entity.Id);
        }

        [Fact]
        public void EntityBase_WithGuidKey_ShouldSetAndGetId()
        {
            // Arrange
            var entity = new TestEntity<Guid>();
            var id = Guid.NewGuid();

            // Act
            entity.Id = id;

            // Assert
            Assert.Equal(id, entity.Id);
        }

        [Fact]
        public void EntityBase_WithStringKey_ShouldSetAndGetId()
        {
            // Arrange
            var entity = new TestEntity<string>();
            var id = "test-id";

            // Act
            entity.Id = id;

            // Assert
            Assert.Equal(id, entity.Id);
        }
    }
}
