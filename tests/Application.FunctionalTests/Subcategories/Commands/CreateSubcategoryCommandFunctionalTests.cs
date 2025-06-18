using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Subcategories.Commands.CreateSubcategoryCommand;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Infrastructure.Data;
using BackendAuthTemplate.Tests.Common.Categories;
using BackendAuthTemplate.Tests.Common.Subcategories;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Subcategories.Commands
{
    public class CreateSubcategoryCommandFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task CreateSubcategoryCommand_WithValidData_ShouldReturnSubcategoryId()
        {
            AppDbContext context = _fixture.ServiceProvider.GetRequiredService<AppDbContext>();
            Category category = await CategoriesEntitiesTestHelper.SeedCategory(context);

            CreateSubcategoryCommand command = SubcategoriesCommandsTestHelper.CreateSubcategoryCommand(categoryId: category.Id);

            Result<Guid> result = await _mediator.Send(command);

            result.Succeeded.ShouldBeTrue();
            result.Value.ShouldNotBe(Guid.Empty);
        }
    }
}
