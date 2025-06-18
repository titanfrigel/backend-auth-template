using BackendAuthTemplate.Application.Common.Behaviors;
using BackendAuthTemplate.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace BackendAuthTemplate.Application.UnitTests.Behaviors
{
    public class LoggingBehaviorTests
    {
        public class TestRequest : IRequest { }

        [Fact]
        public async Task LoggingBehavior_Should_Log_Info_And_Invoke_Next()
        {
            Mock<ILogger<TestRequest>> loggerMock = new();
            Mock<IUser> userContextMock = new();

            _ = userContextMock.Setup(x => x.UserId).Returns(Guid.NewGuid());

            LoggingBehavior<TestRequest> behavior = new(loggerMock.Object, userContextMock.Object);
            TestRequest request = new();

            await behavior.Process(request, CancellationToken.None);

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()
                ),
                Times.Once
            );
        }
    }
}
