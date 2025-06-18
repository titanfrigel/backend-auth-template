using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BackendAuthTemplate.Infrastructure.Data.Interceptors
{
    public class AuditableEntityInterceptor(IUser userContext, TimeProvider timeProvider) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext? context)
        {
            if (context == null)
            {
                return;
            }

            IEnumerable<EntityEntry<IAuditableEntity>> entries = context.ChangeTracker.Entries<IAuditableEntity>()
                    .Where(e => e.State is EntityState.Added or EntityState.Modified);

            foreach (EntityEntry<IAuditableEntity> entry in entries)
            {
                if (userContext == null || !userContext.HasContext)
                {
                    throw new InvalidOperationException("UserContext is required to set audit fields.");
                }

                if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    DateTimeOffset utcNow = timeProvider.GetUtcNow();

                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedById = userContext.UserId;
                        entry.Entity.CreatedAt = utcNow;
                    }

                    entry.Entity.UpdatedById = userContext.UserId;
                    entry.Entity.UpdatedAt = utcNow;
                }
            }
        }
    }

    public static class Extensions
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry)
        {
            return entry.References.Any(r =>
                r.TargetEntry != null &&
                r.TargetEntry.Metadata.IsOwned() &&
                (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
        }
    }
}
