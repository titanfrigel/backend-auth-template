using BackendAuthTemplate.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendAuthTemplate.Domain.Interfaces
{
    public interface IEntity
    {
        Guid Id { get; set; }

        [NotMapped]
        IReadOnlyCollection<BaseEvent> DomainEvents { get; }

        void AddDomainEvent(BaseEvent domainEvent);
        void RemoveDomainEvent(BaseEvent domainEvent);
        void ClearDomainEvents();
    }
}
