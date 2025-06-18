using BackendAuthTemplate.API.Hubs;
using BackendAuthTemplate.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace BackendAuthTemplate.API.EventHandlers.Subcategories
{
    public class SubcategoryCreatedSignalrHandler(IHubContext<SubcategoriesHub> hubContext) : INotificationHandler<SubcategoryCreatedEvent>
    {
        public async Task Handle(SubcategoryCreatedEvent notification, CancellationToken cancellationToken = default)
        {
            await hubContext.Clients.All.SendAsync("SubcategoryCreated", notification.SubcategoryId, cancellationToken);
        }
    }
}
