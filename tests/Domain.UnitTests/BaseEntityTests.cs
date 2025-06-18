using BackendAuthTemplate.Domain.Common;
using Shouldly;

namespace BackendAuthTemplate.Domain.UnitTests
{
    public class TestEntity : BaseEntity { }

    public class BaseEntityTests
    {
        [Fact]
        public void AddDomainEvent_Should_AddEventToCollection()
        {
            TestEntity entity = new();
            TestDomainEvent domainEvent = new();

            entity.AddDomainEvent(domainEvent);

            entity.DomainEvents.ShouldContain(domainEvent);
        }

        [Fact]
        public void RemoveDomainEvent_Should_RemoveEventFromCollection()
        {
            TestEntity entity = new();
            TestDomainEvent domainEvent = new();

            entity.AddDomainEvent(domainEvent);
            entity.RemoveDomainEvent(domainEvent);

            entity.DomainEvents.ShouldNotContain(domainEvent);
        }

        [Fact]
        public void ClearDomainEvents_Should_ClearAllEvents()
        {
            TestEntity entity = new();

            entity.AddDomainEvent(new TestDomainEvent());
            entity.AddDomainEvent(new TestDomainEvent());

            entity.ClearDomainEvents();

            entity.DomainEvents.ShouldBeEmpty();
        }

        private class TestDomainEvent : BaseEvent { }
    }
}
