using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using BackendAuthTemplate.Application.Features.Categories.Queries.GetCategoriesWithPaginationQuery;
using BackendAuthTemplate.Tests.Common.Categories;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Categories.Queries
{
    public class GetCategoriesWithPaginationQueryFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task GetCategoriesWithPaginationQuery_ShouldReturnPaginatedListOfCategories()
        {
            GetCategoriesWithPaginationQuery query = CategoriesQueriesTestHelper.GetCategoriesWithPaginationQuery();

            Result<PaginatedList<ReadCategoryDto>> result = await _mediator.Send(query);

            result.Succeeded.ShouldBeTrue();
        }
    }
}
