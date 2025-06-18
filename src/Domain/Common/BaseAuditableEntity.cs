using BackendAuthTemplate.Domain.Interfaces;

namespace BackendAuthTemplate.Domain.Common
{
    public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
    {
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public Guid CreatedById { get; set; } = Guid.Empty;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public Guid UpdatedById { get; set; } = Guid.Empty;
    }
}
