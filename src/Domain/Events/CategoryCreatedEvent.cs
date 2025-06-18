using BackendAuthTemplate.Domain.Common;

namespace BackendAuthTemplate.Domain.Events
{
    public class CategoryCreatedEvent(Guid categoryId) : BaseEvent
    {
        public Guid CategoryId { get; } = categoryId;
    }
}
