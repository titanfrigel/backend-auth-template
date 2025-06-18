using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using BackendAuthTemplate.Application.Features.Categories.Queries.GetCategoryByIdQuery;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Infrastructure.Data;
using BackendAuthTemplate.Tests.Common.Categories;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Categories.Queries
{
    public class GetCategoryByIdQueryFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task GetCategoryByIdQuery_WithValidId_ShouldReturnCategory()
        {
            AppDbContext context = _fixture.ServiceProvider.GetRequiredService<AppDbContext>();
            Category category = await CategoriesEntitiesTestHelper.SeedCategory(context);

            GetCategoryByIdQuery query = CategoriesQueriesTestHelper.GetCategoryByIdQuery(category.Id);

            Result<ReadCategoryDto> result = await _mediator.Send(query);

            result.Succeeded.ShouldBeTrue();
            _ = result.Value.ShouldNotBeNull();
            result.Value.Id.ShouldBe(category.Id);
        }

        [Fact]
        public async Task GetCategoryByIdQuery_WithInvalidId_ShouldFail()
        {
            GetCategoryByIdQuery query = CategoriesQueriesTestHelper.GetCategoryByIdQuery();

            Result<ReadCategoryDto> result = await _mediator.Send(query);

            result.Succeeded.ShouldBeFalse();
        }
    }
}
