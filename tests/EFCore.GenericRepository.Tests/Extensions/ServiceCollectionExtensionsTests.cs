using EFCore.GenericRepository.Extensions;
using EFCore.GenericRepository.Tests.Fixtures;
using EFCore.GenericRepository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace EFCore.GenericRepository.Tests.Extensions
{
    /// <summary>
    /// Unit tests for ServiceCollectionExtensions.
    /// </summary>
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddGenericRepository_WithOptions_RegistersDbContextAndUnitOfWork()
        {
            // Arrange
            var services = new ServiceCollection();
            var databaseName = Guid.NewGuid().ToString();

            // Act
            services.AddGenericRepository<TestDbContext>(options =>
                options.UseInMemoryDatabase(databaseName));

            // Assert
            var dbContextDescriptor = services.First(descriptor => descriptor.ServiceType == typeof(TestDbContext));
            Assert.Equal(ServiceLifetime.Scoped, dbContextDescriptor.Lifetime);

            using var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TestDbContext>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork<TestDbContext>>();

            Assert.NotNull(context);
            Assert.NotNull(unitOfWork);
            Assert.Same(context, unitOfWork.Context);
        }

        [Fact]
        public void AddGenericRepository_WithOptions_UsesSpecifiedLifetimes()
        {
            // Arrange
            var services = new ServiceCollection();
            var databaseName = Guid.NewGuid().ToString();

            // Act
            services.AddGenericRepository<TestDbContext>(
                options => options.UseInMemoryDatabase(databaseName),
                contextLifetime: ServiceLifetime.Singleton,
                optionsLifetime: ServiceLifetime.Singleton);

            // Assert
            var dbContextDescriptor = services.First(descriptor => descriptor.ServiceType == typeof(TestDbContext));
            var unitOfWorkDescriptor = services.First(descriptor => descriptor.ServiceType == typeof(IUnitOfWork<TestDbContext>));

            Assert.Equal(ServiceLifetime.Singleton, dbContextDescriptor.Lifetime);
            Assert.Equal(ServiceLifetime.Singleton, unitOfWorkDescriptor.Lifetime);
        }

        [Fact]
        public void AddGenericRepository_WithoutOptions_UsesExistingDbContextRegistration()
        {
            // Arrange
            var services = new ServiceCollection();
            var databaseName = Guid.NewGuid().ToString();
            services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase(databaseName));

            // Act
            services.AddGenericRepository<TestDbContext>(ServiceLifetime.Transient);

            // Assert
            var unitOfWorkDescriptor = services.First(descriptor => descriptor.ServiceType == typeof(IUnitOfWork<TestDbContext>));
            Assert.Equal(ServiceLifetime.Transient, unitOfWorkDescriptor.Lifetime);

            using var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TestDbContext>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork<TestDbContext>>();

            Assert.NotNull(context);
            Assert.NotNull(unitOfWork);
            Assert.Same(context, unitOfWork.Context);
        }

        [Fact]
        public void AddGenericRepository_Parameterless_DefaultsToScopedLifetime()
        {
            // Arrange
            var services = new ServiceCollection();
            var databaseName = Guid.NewGuid().ToString();
            services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase(databaseName));

            // Act
            services.AddGenericRepository<TestDbContext>();

            // Assert
            var unitOfWorkDescriptor = services.First(descriptor => descriptor.ServiceType == typeof(IUnitOfWork<TestDbContext>));
            Assert.Equal(ServiceLifetime.Scoped, unitOfWorkDescriptor.Lifetime);
        }

        [Fact]
        public void AddGenericRepository_ShouldThrowWhenServicesIsNull_ForOptionsOverload()
        {
            // Arrange
            IServiceCollection? services = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                ServiceCollectionExtensions.AddGenericRepository<TestDbContext>(services!, options => options.UseInMemoryDatabase("TestDb")));
        }

        [Fact]
        public void AddGenericRepository_ShouldThrowWhenOptionsActionIsNull()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                services.AddGenericRepository<TestDbContext>(optionsAction: null!));
        }

        [Fact]
        public void AddGenericRepository_ShouldThrowWhenServicesIsNull_ForExistingDbContextOverload()
        {
            // Arrange
            IServiceCollection? services = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                ServiceCollectionExtensions.AddGenericRepository<TestDbContext>(services!));
        }
    }
}
