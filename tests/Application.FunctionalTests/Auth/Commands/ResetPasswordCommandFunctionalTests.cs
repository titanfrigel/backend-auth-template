using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Auth.Commands.ResetPasswordCommand;
using BackendAuthTemplate.Tests.Common.Auth;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Auth.Commands
{
    public class ResetPasswordCommandFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task ResetPasswordCommand_Should_Return_Failure_If_User_NotFound()
        {
            ResetPasswordCommand command = AuthCommandsTestHelper.ResetPasswordCommand();

            Result result = await _mediator.Send(command);

            result.Succeeded.ShouldBeFalse();
        }
    }
}
