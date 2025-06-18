namespace BackendAuthTemplate.Domain.Interfaces
{
    public interface ISoftDeleteEntity : IEntity
    {
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public Guid? DeletedById { get; set; }
    }
}
