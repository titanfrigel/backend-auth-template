using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.FeatureName.Commands.CommandNameCommand;
using BackendAuthTemplate.Tests.Common.FeatureName;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.FeatureName.Commands
{
    public class CommandNameCommandFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task CommandNameCommand_WithValidName_ShouldReturnId()
        {
            CommandNameCommand command = FeatureNameCommandsTestHelper.CommandNameCommand();

            Result<Guid> result = await _mediator.Send(command);

            result.Succeeded.ShouldBeTrue();
            result.Value.ShouldNotBe(Guid.Empty);
        }
    }
}

// public static CommandNameCommand CommandNameCommand(
//     string name = "Default"
// )
// {
//     return new()
//     {
//         Name = name
//     };
// }