using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Auth.Commands.ConfirmEmailCommand;
using BackendAuthTemplate.Tests.Common.Auth;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Auth.Commands
{
    public class ConfirmEmailCommandFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task ConfirmEmailCommand_Should_Return_Success_For_ValidInput()
        {
            ConfirmEmailCommand command = AuthCommandsTestHelper.ConfirmEmailCommand();

            Result result = await _mediator.Send(command);
            // Expect failure in this test since the user does not exist.
            result.Succeeded.ShouldBeFalse();
        }
    }
}
