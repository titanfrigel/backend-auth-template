namespace AuthTemplate.Db.Models
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid CreatedById { get; set; } = Guid.Empty;
        public AppUser CreatedBy { get; set; } = null!;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Guid UpdatedById { get; set; } = Guid.Empty;
        public AppUser UpdatedBy { get; set; } = null!;
    }
}
