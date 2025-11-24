using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.FeatureName.Commands.CreateEntityNameCommand;
using BackendAuthTemplate.Tests.Common.FeatureName;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.FeatureName.Commands
{
    public class CreateEntityNameCommandFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task CreateFeatureNameCommand_WithValidName_ShouldReturnFeatureNameId()
        {
            CreateEntityNameCommand command = FeatureNameCommandsTestHelper.CreateEntityNameCommand();

            Result<Guid> result = await _mediator.Send(command);

            result.Succeeded.ShouldBeTrue();
            result.Value.ShouldNotBe(Guid.Empty);
        }
    }
}
