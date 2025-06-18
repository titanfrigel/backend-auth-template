using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Users.Commands.UpdateUserMeCommand;
using BackendAuthTemplate.Tests.Common.Users;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Users.Commands
{
    public class UpdateUserMeCommandFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task UpdateUserMeCommand_Should_Fail()
        {
            UpdateUserMeCommand command = UsersCommandsTestHelper.UpdateUserMeCommand();

            Result result = await _mediator.Send(command);

            result.Succeeded.ShouldBeFalse();
        }
    }
}
