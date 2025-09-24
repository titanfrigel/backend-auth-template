using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using BackendAuthTemplate.Application.Features.Subcategories.Queries.GetAllSubcategoriesWithPaginationQuery;
using BackendAuthTemplate.Tests.Common.Subcategories;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Subcategories.Queries
{
    public class GetAllSubcategoriesWithPaginationQueryFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task GetAllSubcategoriesWithPaginationQuery_ShouldReturnPaginatedList()
        {
            GetAllSubcategoriesWithPaginationQuery query = SubcategoriesQueriesTestHelper.GetAllSubcategoriesWithPaginationQuery();

            Result<PaginatedList<ReadSubcategoryDto>> result = await _mediator.Send(query);

            result.Succeeded.ShouldBeTrue();
        }
    }
}
