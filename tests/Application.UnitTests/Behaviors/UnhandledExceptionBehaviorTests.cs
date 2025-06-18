using BackendAuthTemplate.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BackendAuthTemplate.Application.UnitTests.Behaviors
{
    public class UnhandledExceptionBehaviorTests
    {
        public class TestRequest : IRequest<string> { }

        [Fact]
        public async Task UnhandledExceptionBehavior_Should_Log_Error_And_Throw_Exception()
        {
            Mock<ILogger<TestRequest>> loggerMock = new();
            UnhandledExceptionBehavior<TestRequest, string> behavior = new(loggerMock.Object);
            TestRequest request = new();

            static Task<string> FailingDelegate(CancellationToken t)
            {
                throw new InvalidOperationException("Test exception");
            }

            Func<Task> act = async () => await behavior.Handle(request, FailingDelegate, CancellationToken.None);

            _ = await act.ShouldThrowAsync<InvalidOperationException>();

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
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
