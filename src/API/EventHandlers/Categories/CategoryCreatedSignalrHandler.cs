using BackendAuthTemplate.API.Hubs;
using BackendAuthTemplate.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace BackendAuthTemplate.API.EventHandlers.Categories
{
    public class CategoryCreatedSignalrHandler(IHubContext<CategoriesHub> hubContext) : INotificationHandler<CategoryCreatedEvent>
    {
        public async Task Handle(CategoryCreatedEvent notification, CancellationToken cancellationToken = default)
        {
            await hubContext.Clients.All.SendAsync("CategoryCreated", notification.CategoryId, cancellationToken);
        }
    }
}
