using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.FeatureName.Dtos;
using BackendAuthTemplate.Application.Features.FeatureName.Queries.GetFeatureNameWithPaginationQuery;
using BackendAuthTemplate.Tests.Common.FeatureName;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.FeatureName.Queries
{
    public class GetFeatureNameWithPaginationQueryFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task GetFeatureNameWithPaginationQuery_ShouldReturnPaginatedListOfFeatureName()
        {
            GetFeatureNameWithPaginationQuery query = FeatureNameQueriesTestHelper.GetFeatureNameWithPaginationQuery();

            Result<PaginatedList<ReadEntityNameDto>> result = await _mediator.Send(query);

            result.Succeeded.ShouldBeTrue();
        }
    }
}
