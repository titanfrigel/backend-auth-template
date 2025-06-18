using BackendAuthTemplate.Domain.Common;
using BackendAuthTemplate.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace BackendAuthTemplate.Infrastructure.Data.Interceptors
{

    public class DispatchDomainEventsInterceptor(IMediator mediator, ILogger<DispatchDomainEventsInterceptor> logger) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();

            return base.SavingChanges(eventData, result);

        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await DispatchDomainEvents(eventData.Context);

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public async Task DispatchDomainEvents(DbContext? context)
        {
            if (context == null)
            {
                return;
            }

            IEnumerable<IEntity> entities = context.ChangeTracker
                .Entries<IEntity>()
                .Where(e => e.Entity.DomainEvents.Count != 0)
                .Select(e => e.Entity);

            List<BaseEvent> domainEvents = entities
                .SelectMany(e => e.DomainEvents)
                .ToList();

            entities.ToList().ForEach(e => e.ClearDomainEvents());

            foreach (BaseEvent? domainEvent in domainEvents)
            {
                try
                {
                    await mediator.Publish(domainEvent);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while dispatching domain event {DomainEvent}", domainEvent.GetType().Name);
                }
            }
        }
    }
}