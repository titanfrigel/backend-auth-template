using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Auth.Commands.RefreshTokenCommand;
using BackendAuthTemplate.Application.Features.Auth.Dtos;
using BackendAuthTemplate.Tests.Common.Auth;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Auth.Commands
{
    public class RefreshTokenCommandFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task RefreshTokenCommand_Should_Return_Failure_When_No_RefreshToken()
        {
            RefreshTokenCommand command = AuthCommandsTestHelper.RefreshTokenCommand();

            Result<ReadTokenDto> result = await _mediator.Send(command);

            result.Succeeded.ShouldBeFalse();
        }
    }
}
