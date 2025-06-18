using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Subcategories.Commands.UpdateSubcategoryCommand;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Infrastructure.Data;
using BackendAuthTemplate.Tests.Common.Categories;
using BackendAuthTemplate.Tests.Common.Subcategories;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Subcategories.Commands
{
    public class UpdateSubcategoryCommandFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task UpdateSubcategoryCommand_WithValidData_ShouldSucceed()
        {
            AppDbContext context = _fixture.ServiceProvider.GetRequiredService<AppDbContext>();
            Subcategory subcategory = await SubcategoriesEntitiesTestHelper.SeedSubcategory(context);

            UpdateSubcategoryCommand updateCommand = SubcategoriesCommandsTestHelper.UpdateSubcategoryCommand(id: subcategory.Id, categoryId: subcategory.CategoryId);

            Result updateResult = await _mediator.Send(updateCommand);

            updateResult.Succeeded.ShouldBeTrue();
            (await context.Subcategories.FindAsync(updateCommand.SubcategoryId)).ShouldNotBeNull().Name.ShouldBe(updateCommand.Name);
        }

        [Fact]
        public async Task UpdateSubcategoryCommand_WithInvalidId_ShouldFail()
        {
            AppDbContext context = _fixture.ServiceProvider.GetRequiredService<AppDbContext>();

            Category category = await CategoriesEntitiesTestHelper.SeedCategory(context);

            UpdateSubcategoryCommand updateCommand = SubcategoriesCommandsTestHelper.UpdateSubcategoryCommand(categoryId: category.Id);

            Result updateResult = await _mediator.Send(updateCommand);

            updateResult.Succeeded.ShouldBeFalse();
        }
    }
}
