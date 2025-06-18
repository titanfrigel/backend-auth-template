using BackendAuthTemplate.Domain.Interfaces;

namespace BackendAuthTemplate.Domain.Common
{
    public abstract class BaseSoftDeleteAuditableEntity : BaseEntity, IAuditableEntity, ISoftDeleteEntity
    {
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public Guid CreatedById { get; set; } = Guid.Empty;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public Guid UpdatedById { get; set; } = Guid.Empty;

        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset? DeletedAt { get; set; } = null;
        public Guid? DeletedById { get; set; } = null;
    }
}
