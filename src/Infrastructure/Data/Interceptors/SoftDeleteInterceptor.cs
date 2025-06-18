using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BackendAuthTemplate.Infrastructure.Data.Interceptors
{
    public class SoftDeleteInterceptor(IUser userContext, TimeProvider timeProvider) : SaveChangesInterceptor
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

            IEnumerable<EntityEntry<ISoftDeleteEntity>> entries = context.ChangeTracker.Entries<ISoftDeleteEntity>()
                    .Where(e => e.State is EntityState.Deleted);

            foreach (EntityEntry<ISoftDeleteEntity> entry in entries)
            {
                if (userContext == null || !userContext.HasContext)
                {
                    throw new InvalidOperationException("UserContext is required to delete an entity with soft delete.");
                }

                DateTimeOffset utcNow = timeProvider.GetUtcNow();

                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedById = userContext.UserId;
                entry.Entity.DeletedAt = utcNow;
            }
        }
    }
}

