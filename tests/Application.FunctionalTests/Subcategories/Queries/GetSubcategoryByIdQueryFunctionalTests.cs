using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using BackendAuthTemplate.Application.Features.Subcategories.Queries.GetSubcategoryByIdQuery;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Infrastructure.Data;
using BackendAuthTemplate.Tests.Common.Subcategories;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Subcategories.Queries
{
    public class GetSubcategoryByIdQueryFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task GetSubcategoryByIdQuery_WithValidId_ShouldReturnSubcategory()
        {
            AppDbContext context = _fixture.ServiceProvider.GetRequiredService<AppDbContext>();
            Subcategory subcategory = await SubcategoriesEntitiesTestHelper.SeedSubcategory(context);

            GetSubcategoryByIdQuery query = SubcategoriesQueriesTestHelper.GetSubcategoryByIdQuery(id: subcategory.Id);

            Result<ReadSubcategoryDto> result = await _mediator.Send(query);

            result.Succeeded.ShouldBeTrue();
            _ = result.Value.ShouldNotBeNull();
            result.Value.Id.ShouldBe(subcategory.Id);
        }

        [Fact]
        public async Task GetSubcategoryByIdQuery_WithInvalidId_ShouldFail()
        {
            GetSubcategoryByIdQuery query = SubcategoriesQueriesTestHelper.GetSubcategoryByIdQuery();

            Result<ReadSubcategoryDto> result = await _mediator.Send(query);

            result.Succeeded.ShouldBeFalse();
        }
    }
}
