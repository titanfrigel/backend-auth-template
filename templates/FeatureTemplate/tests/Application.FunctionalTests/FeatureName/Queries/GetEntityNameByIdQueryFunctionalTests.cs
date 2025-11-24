using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.FeatureName.Dtos;
using BackendAuthTemplate.Application.Features.FeatureName.Queries.GetEntityNameByIdQuery;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using BackendAuthTemplate.Tests.Common.FeatureName;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.FeatureName.Queries
{
    public class GetEntityNameByIdQueryFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task GetEntityNameByIdQuery_WithValidId_ShouldReturnEntityName()
        {
            AppDbContext context = _fixture.ServiceProvider.GetRequiredService<AppDbContext>();
            EntityName entityName = await FeatureNameEntitiesTestHelper.SeedEntityName(context);

            GetEntityNameByIdQuery query = FeatureNameQueriesTestHelper.GetEntityNameByIdQuery(entityName.Id);

            Result<ReadEntityNameDto> result = await _mediator.Send(query);

            result.Succeeded.ShouldBeTrue();
            _ = result.Value.ShouldNotBeNull();
            result.Value.Id.ShouldBe(entityName.Id);
        }

        [Fact]
        public async Task GetEntityNameByIdQuery_WithInvalidId_ShouldFail()
        {
            GetEntityNameByIdQuery query = FeatureNameQueriesTestHelper.GetEntityNameByIdQuery();

            Result<ReadEntityNameDto> result = await _mediator.Send(query);

            result.Succeeded.ShouldBeFalse();
        }
    }
}
