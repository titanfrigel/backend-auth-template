using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.FeatureName.Commands.UpdateEntityNameCommand;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using BackendAuthTemplate.Tests.Common.FeatureName;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.FeatureName.Commands
{
    public class UpdateFeatureNameCommandFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task UpdateFeatureNameCommand_WithValidName_ShouldReturnSuccess()
        {
            AppDbContext context = _fixture.ServiceProvider.GetRequiredService<AppDbContext>();
            EntityName entityName = await FeatureNameEntitiesTestHelper.SeedEntityName(context);

            UpdateEntityNameCommand command = FeatureNameCommandsTestHelper.UpdateEntityNameCommand(entityName.Id);

            Result result = await _mediator.Send(command);

            result.Succeeded.ShouldBeTrue();
        }
    }
}
