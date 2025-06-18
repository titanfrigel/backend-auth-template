namespace BackendAuthTemplate.Domain.Interfaces
{
    public interface IAuditableEntity : IEntity
    {
        public DateTimeOffset CreatedAt { get; set; }
        public Guid CreatedById { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public Guid UpdatedById { get; set; }
    }
}
