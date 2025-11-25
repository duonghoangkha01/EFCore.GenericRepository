using EFCore.GenericRepository.Entities;
using System.Collections.Generic;
using Xunit;

namespace EFCore.GenericRepository.Tests.Entities
{
    public class PagedResultTests
    {
        [Fact]
        public void TotalPages_ShouldCalculateCorrectly_WhenTotalCountIsDivisibleByPageSize()
        {
            // Arrange
            var pagedResult = new PagedResult<string>
            {
                TotalCount = 100,
                PageSize = 10
            };

            // Act
            var totalPages = pagedResult.TotalPages;

            // Assert
            Assert.Equal(10, totalPages);
        }

        [Fact]
        public void TotalPages_ShouldCalculateCorrectly_WhenTotalCountIsNotDivisibleByPageSize()
        {
            // Arrange
            var pagedResult = new PagedResult<string>
            {
                TotalCount = 101,
                PageSize = 10
            };

            // Act
            var totalPages = pagedResult.TotalPages;

            // Assert
            Assert.Equal(11, totalPages);
        }

        [Fact]
        public void TotalPages_ShouldBeZero_WhenPageSizeIsZero()
        {
            // Arrange
            var pagedResult = new PagedResult<string>
            {
                TotalCount = 100,
                PageSize = 0
            };

            // Act
            var totalPages = pagedResult.TotalPages;

            // Assert
            Assert.Equal(0, totalPages);
        }
        
        [Fact]
        public void TotalPages_ShouldBeZero_WhenTotalCountIsZero()
        {
            // Arrange
            var pagedResult = new PagedResult<string>
            {
                TotalCount = 0,
                PageSize = 10
            };

            // Act
            var totalPages = pagedResult.TotalPages;

            // Assert
            Assert.Equal(0, totalPages);
        }

        [Theory]
        [InlineData(1, 10, false)]
        [InlineData(2, 10, true)]
        [InlineData(10, 10, true)]
        public void HasPreviousPage_ShouldBeCorrect(int pageNumber, int totalPages, bool expected)
        {
            // Arrange
            var pagedResult = new PagedResult<string>
            {
                PageNumber = pageNumber,
                TotalCount = totalPages * 10,
                PageSize = 10
            };

            // Act & Assert
            Assert.Equal(expected, pagedResult.HasPreviousPage);
        }
        
        [Theory]
        [InlineData(1, 10, true)]
        [InlineData(9, 10, true)]
        [InlineData(10, 10, false)]
        public void HasNextPage_ShouldBeCorrect(int pageNumber, int totalPages, bool expected)
        {
            // Arrange
            var pagedResult = new PagedResult<string>
            {
                PageNumber = pageNumber,
                TotalCount = totalPages * 10,
                PageSize = 10
            };

            // Act & Assert
            Assert.Equal(expected, pagedResult.HasNextPage);
        }
    }
}
