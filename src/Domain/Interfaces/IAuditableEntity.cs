namespace BackendAuthTemplate.Domain.Interfaces
{
    public interface IAuditableEntity : IEntity
    {
        DateTimeOffset CreatedAt { get; set; }
        Guid CreatedById { get; set; }
        DateTimeOffset UpdatedAt { get; set; }
        Guid UpdatedById { get; set; }
    }
}
