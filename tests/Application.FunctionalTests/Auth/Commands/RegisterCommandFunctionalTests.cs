using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Auth.Commands.RegisterCommand;
using BackendAuthTemplate.Tests.Common.Auth;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Auth.Commands
{
    public class RegisterCommandFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task RegisterCommand_Should_Return_Success()
        {
            RegisterCommand command = AuthCommandsTestHelper.RegisterCommand();

            Result result = await _mediator.Send(command);

            result.Succeeded.ShouldBeTrue();
        }
    }
}
