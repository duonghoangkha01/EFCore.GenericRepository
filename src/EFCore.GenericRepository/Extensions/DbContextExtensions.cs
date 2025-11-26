using EFCore.GenericRepository.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EFCore.GenericRepository.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="DbContext"/> to configure soft delete functionality.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Configures global query filters for soft delete functionality.
        /// This method applies a global filter to all entities implementing <see cref="ISoftDeletable"/>,
        /// automatically excluding soft-deleted entities from queries unless explicitly included with IgnoreQueryFilters().
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure the entity types.</param>
        /// <remarks>
        /// This method should be called in the <see cref="DbContext.OnModelCreating"/> method.
        /// After applying this configuration, all queries will automatically filter out entities where IsDeleted = true.
        /// To include soft-deleted entities in a query, use the IgnoreQueryFilters() method.
        /// <example>
        /// <code>
        /// protected override void OnModelCreating(ModelBuilder modelBuilder)
        /// {
        ///     base.OnModelCreating(modelBuilder);
        ///     modelBuilder.ConfigureSoftDelete();
        /// }
        /// </code>
        /// </example>
        /// </remarks>
        public static void ConfigureSoftDelete(this ModelBuilder modelBuilder)
        {
            // Get all entity types that implement ISoftDeletable
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Check if the entity type implements ISoftDeletable
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    // Create a parameter expression for the entity
                    var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");

                    // Create the property access expression: e.IsDeleted
                    var property = System.Linq.Expressions.Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));

                    // Create the comparison expression: e.IsDeleted == false
                    var comparison = System.Linq.Expressions.Expression.Equal(
                        property,
                        System.Linq.Expressions.Expression.Constant(false));

                    // Create the lambda expression: e => e.IsDeleted == false
                    var lambda = System.Linq.Expressions.Expression.Lambda(comparison, parameter);

                    // Apply the query filter to the entity type
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }
    }
}
