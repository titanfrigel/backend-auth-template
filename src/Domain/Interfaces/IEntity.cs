using BackendAuthTemplate.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendAuthTemplate.Domain.Interfaces
{
    public interface IEntity
    {
        public Guid Id { get; set; }

        [NotMapped]
        public IReadOnlyCollection<BaseEvent> DomainEvents { get; }

        public void AddDomainEvent(BaseEvent domainEvent);
        public void RemoveDomainEvent(BaseEvent domainEvent);
        public void ClearDomainEvents();
    }
}
