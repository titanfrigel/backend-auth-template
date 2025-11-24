using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.FeatureName.Dtos;
using BackendAuthTemplate.Application.Features.FeatureName.Queries.QueryNameQuery;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using BackendAuthTemplate.Tests.Common.FeatureName;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.FeatureName.Queries
{
    public class QueryNameQueryFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task QueryNameQuery_WithValidId_ShouldReturnDto()
        {
            AppDbContext context = _fixture.ServiceProvider.GetRequiredService<AppDbContext>();
            EntityName entityName = await FeatureNameEntitiesTestHelper.SeedEntityName(context);

            QueryNameQuery query = FeatureNameQueriesTestHelper.QueryNameQuery(entityName.Id);

            Result<ReadEntityNameDto> result = await _mediator.Send(query);

            result.Succeeded.ShouldBeTrue();
            _ = result.Value.ShouldNotBeNull();
            result.Value.Id.ShouldBe(entityName.Id);
        }
    }
}

// public static QueryNameQuery QueryNameQuery(
//     Guid? id = null
// )
// {
//     return new()
//     {
//         EntityNameId = id ?? Guid.NewGuid()
//     };
// }