using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.FeatureName.Commands.DeleteEntityNameCommand;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using BackendAuthTemplate.Tests.Common.FeatureName;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.FeatureName.Commands
{
    public class DeleteFeatureNameCommandFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task DeleteFeatureNameCommand_WithValidId_ShouldReturnSuccess()
        {
            AppDbContext context = _fixture.ServiceProvider.GetRequiredService<AppDbContext>();
            EntityName entityName = await FeatureNameEntitiesTestHelper.SeedEntityName(context);

            DeleteEntityNameCommand command = FeatureNameCommandsTestHelper.DeleteEntityNameCommand(entityName.Id);

            Result result = await _mediator.Send(command);

            result.Succeeded.ShouldBeTrue();
        }
    }
}
