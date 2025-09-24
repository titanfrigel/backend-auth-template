using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using BackendAuthTemplate.Application.Features.Categories.Queries.GetAllCategoriesQuery;
using BackendAuthTemplate.Tests.Common.Categories;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Categories.Queries
{
    public class GetAllCategoriesQueryFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task GetAllCategoriesQuery_ShouldReturnListOfCategories()
        {
            GetAllCategoriesQuery query = CategoriesQueriesTestHelper.GetAllCategoriesQuery();

            Result<List<ReadCategoryDto>> result = await _mediator.Send(query);

            result.Succeeded.ShouldBeTrue();
        }
    }
}
