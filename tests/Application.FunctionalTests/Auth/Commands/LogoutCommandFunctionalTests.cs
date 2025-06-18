using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Auth.Commands.LogoutCommand;
using BackendAuthTemplate.Tests.Common.Auth;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Auth.Commands
{
    public class LogoutCommandFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task LogoutCommand_Should_Return_Success()
        {
            LogoutCommand command = AuthCommandsTestHelper.LogoutCommand();

            Result result = await _mediator.Send(command);

            result.Succeeded.ShouldBeTrue();
        }
    }
}
