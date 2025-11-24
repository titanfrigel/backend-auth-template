using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.FeatureName.Dtos;
using BackendAuthTemplate.Application.Features.FeatureName.Queries.GetAllFeatureNameWithPaginationQuery;
using BackendAuthTemplate.Tests.Common.FeatureName;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.FeatureName.Queries
{
    public class GetAllFeatureNameWithPaginationQueryFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task GetAllFeatureNameWithPaginationQuery_ShouldReturnPaginatedListOfFeatureName()
        {
            GetAllFeatureNameWithPaginationQuery query = FeatureNameQueriesTestHelper.GetAllFeatureNameWithPaginationQuery();

            Result<PaginatedList<ReadEntityNameDto>> result = await _mediator.Send(query);

            result.Succeeded.ShouldBeTrue();
        }
    }
}
