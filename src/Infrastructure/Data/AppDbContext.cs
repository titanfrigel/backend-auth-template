using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Domain.Interfaces;
using BackendAuthTemplate.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace BackendAuthTemplate.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options), IAppDbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Prevents cascade delete
            foreach (IMutableForeignKey? relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // Remove soft deleted entity from queries
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeleteEntity).IsAssignableFrom(entityType.ClrType))
                {
                    ParameterExpression parameter = Expression.Parameter(entityType.ClrType, "e");

                    MemberExpression isDeletedProperty = Expression.Property(parameter, nameof(ISoftDeleteEntity.IsDeleted));

                    BinaryExpression filterCondition = Expression.Equal(isDeletedProperty, Expression.Constant(false));

                    LambdaExpression lambda = Expression.Lambda(filterCondition, parameter);

                    _ = modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }

            _ = modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
