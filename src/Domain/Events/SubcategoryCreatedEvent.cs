using BackendAuthTemplate.Domain.Common;

namespace BackendAuthTemplate.Domain.Events
{
    public class SubcategoryCreatedEvent(Guid subcategoryId) : BaseEvent
    {
        public Guid SubcategoryId { get; } = subcategoryId;
    }
}
