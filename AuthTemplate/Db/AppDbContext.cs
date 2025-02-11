using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AuthTemplate.Db.Models;
using AuthTemplate.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AuthTemplate.Db
{
    public class AppDbContext(DbContextOptions<AppDbContext> options, UserContext userContext) : IdentityDbContext<AppUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            SetAuditFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetAuditFields()
        {
            IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity>> entries = ChangeTracker.Entries<AuditableEntity>()
                .Where(e => e.State is EntityState.Added or EntityState.Modified);

            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in entries)
            {
                if (userContext == null || !userContext.HasContext)
                {
                    throw new InvalidOperationException("UserContext is required to set audit fields.");
                }

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedById = userContext.UserId;
                    entry.Entity.CreatedAt = DateTime.UtcNow;

                    entry.Entity.UpdatedById = entry.Entity.CreatedById;
                    entry.Entity.UpdatedAt = entry.Entity.CreatedAt;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedById = userContext.UserId;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
