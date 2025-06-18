using BackendAuthTemplate.API.EventHandlers.Subcategories;
using BackendAuthTemplate.API.Hubs;
using BackendAuthTemplate.Domain.Events;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace BackendAuthTemplate.API.UnitTests.EventHandlers
{
    public class SubcategoryCreatedSignalrHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_SignalR_With_SubcategoryCreated()
        {
            Mock<IHubContext<SubcategoriesHub>> hubContextMock = new();
            Mock<IHubClients> clientsMock = new();
            Mock<IClientProxy> clientProxyMock = new();

            _ = hubContextMock.Setup(x => x.Clients).Returns(clientsMock.Object);
            _ = clientsMock.Setup(x => x.All).Returns(clientProxyMock.Object);

            SubcategoryCreatedSignalrHandler handler = new(hubContextMock.Object);
            SubcategoryCreatedEvent subcategoryEvent = new(Guid.NewGuid());

            await handler.Handle(subcategoryEvent, CancellationToken.None);

            clientProxyMock.Verify(x => x.SendCoreAsync("SubcategoryCreated",
                    It.Is<object[]>(args => (Guid)args[0] == subcategoryEvent.SubcategoryId),
                    It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
    }
}
