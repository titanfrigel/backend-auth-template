using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using BackendAuthTemplate.Application.Features.Categories.Queries.GetAllCategoriesWithPaginationQuery;
using BackendAuthTemplate.Tests.Common.Categories;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Categories.Queries
{
    public class GetAllCategoriesWithPaginationQueryFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task GetAllCategoriesWithPaginationQuery_ShouldReturnPaginatedListOfCategories()
        {
            GetAllCategoriesWithPaginationQuery query = CategoriesQueriesTestHelper.GetAllCategoriesWithPaginationQuery();

            Result<PaginatedList<ReadCategoryDto>> result = await _mediator.Send(query);

            result.Succeeded.ShouldBeTrue();
        }
    }
}
