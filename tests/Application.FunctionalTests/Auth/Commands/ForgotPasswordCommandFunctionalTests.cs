using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Auth.Commands.ForgotPasswordCommand;
using BackendAuthTemplate.Tests.Common.Auth;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Auth.Commands
{
    public class ForgotPasswordCommandFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task ForgotPasswordCommand_Should_Return_Success_If_User_NotFound()
        {
            ForgotPasswordCommand command = AuthCommandsTestHelper.ForgotPasswordCommand();

            Result result = await _mediator.Send(command);

            result.Succeeded.ShouldBeTrue();
        }
    }
}
