using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using BackendAuthTemplate.Application.Features.Subcategories.Queries.GetAllSubcategoriesQuery;
using BackendAuthTemplate.Tests.Common.Subcategories;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Subcategories.Queries
{
    public class GetAllSubcategoriesQueryFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task GetAllSubcategoriesQuery_ShouldReturnList()
        {
            GetAllSubcategoriesQuery query = SubcategoriesQueriesTestHelper.GetAllSubcategoriesQuery();

            Result<List<ReadSubcategoryDto>> result = await _mediator.Send(query);

            result.Succeeded.ShouldBeTrue();
        }
    }
}
