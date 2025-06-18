using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Auth.Commands.LoginCommand;
using BackendAuthTemplate.Application.Features.Auth.Dtos;
using BackendAuthTemplate.Tests.Common.Auth;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Auth.Commands
{
    public class LoginCommandFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task LoginCommand_Should_Return_Token_Or_Failure()
        {
            LoginCommand command = AuthCommandsTestHelper.LoginCommand();

            Result<ReadTokenDto> result = await _mediator.Send(command);

            result.Succeeded.ShouldBeTrue();
            _ = result.Value.ShouldNotBeNull();
            result.Value.AccessToken.ShouldNotBeNullOrEmpty();
        }
    }
}
