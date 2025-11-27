using EFCore.GenericRepository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EFCore.GenericRepository.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> to register generic repository services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the generic repository pattern services including DbContext and Unit of Work.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type to register.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="optionsAction">An action to configure the DbContext options.</param>
        /// <param name="contextLifetime">The lifetime for the DbContext. Defaults to Scoped.</param>
        /// <param name="optionsLifetime">The lifetime for the DbContextOptions. Defaults to Scoped.</param>
        /// <returns>The service collection for method chaining.</returns>
        /// <remarks>
        /// This method registers the following services:
        /// <list type="bullet">
        /// <item><description>DbContext (<typeparamref name="TContext"/>) - Scoped by default</description></item>
        /// <item><description>IUnitOfWork&lt;<typeparamref name="TContext"/>&gt; - Scoped by default</description></item>
        /// </list>
        /// Repositories are created on-demand by the Unit of Work and don't need explicit registration.
        /// <example>
        /// <code>
        /// // Example 1: Register with SQL Server
        /// services.AddGenericRepository&lt;MyDbContext&gt;(options =>
        ///     options.UseSqlServer(connectionString));
        ///
        /// // Example 2: Register with SQLite
        /// services.AddGenericRepository&lt;MyDbContext&gt;(options =>
        ///     options.UseSqlite("Data Source=mydb.db"));
        ///
        /// // Example 3: Register with in-memory database (for testing)
        /// services.AddGenericRepository&lt;MyDbContext&gt;(options =>
        ///     options.UseInMemoryDatabase("TestDb"));
        /// </code>
        /// </example>
        /// </remarks>
        public static IServiceCollection AddGenericRepository<TContext>(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> optionsAction,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where TContext : DbContext
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (optionsAction is null)
            {
                throw new ArgumentNullException(nameof(optionsAction));
            }

            services.AddDbContext<TContext>(optionsAction, contextLifetime, optionsLifetime);
            services.AddUnitOfWork<TContext>(contextLifetime);

            return services;
        }

        /// <summary>
        /// Registers the generic repository pattern services for an already registered DbContext.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="lifetime">The lifetime for the Unit of Work. Defaults to Scoped (should match DbContext lifetime).</param>
        /// <returns>The service collection for method chaining.</returns>
        /// <remarks>
        /// Use this overload when your DbContext is already registered in the service collection.
        /// The Unit of Work will resolve the existing DbContext instance.
        /// <example>
        /// <code>
        /// // First register DbContext separately
        /// services.AddDbContext&lt;MyDbContext&gt;(options =>
        ///     options.UseSqlServer(connectionString));
        ///
        /// // Then register the repository pattern
        /// services.AddGenericRepository&lt;MyDbContext&gt;();
        /// </code>
        /// </example>
        /// </remarks>
        public static IServiceCollection AddGenericRepository<TContext>(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TContext : DbContext
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddUnitOfWork<TContext>(lifetime);

            return services;
        }

        /// <summary>
        /// Adds the UnitOfWork for the specified DbContext type.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="lifetime">The lifetime for the UnitOfWork.</param>
        private static void AddUnitOfWork<TContext>(this IServiceCollection services, ServiceLifetime lifetime)
            where TContext : DbContext
        {
            // Register UnitOfWork with the provided lifetime
            services.Add(new ServiceDescriptor(
                typeof(IUnitOfWork<TContext>),
                serviceProvider =>
                {
                    var context = serviceProvider.GetRequiredService<TContext>();
                    return new UnitOfWork<TContext>(context);
                },
                lifetime));
        }
    }
}
