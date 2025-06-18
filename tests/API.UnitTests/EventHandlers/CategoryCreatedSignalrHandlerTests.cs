using BackendAuthTemplate.API.EventHandlers.Categories;
using BackendAuthTemplate.API.Hubs;
using BackendAuthTemplate.Domain.Events;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace BackendAuthTemplate.API.UnitTests.EventHandlers
{
    public class CategoryCreatedSignalrHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_SignalR_With_CategoryCreated()
        {
            Mock<IHubContext<CategoriesHub>> hubContextMock = new();
            Mock<IHubClients> clientsMock = new();
            Mock<IClientProxy> clientProxyMock = new();

            _ = hubContextMock.Setup(x => x.Clients).Returns(clientsMock.Object);
            _ = clientsMock.Setup(x => x.All).Returns(clientProxyMock.Object);

            CategoryCreatedSignalrHandler handler = new(hubContextMock.Object);
            CategoryCreatedEvent categoryEvent = new(Guid.NewGuid());

            await handler.Handle(categoryEvent, CancellationToken.None);

            clientProxyMock.Verify(x => x.SendCoreAsync("CategoryCreated",
                    It.Is<object[]>(args => (Guid)args[0] == categoryEvent.CategoryId),
                    It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
    }
}
