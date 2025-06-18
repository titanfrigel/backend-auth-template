using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Commands.UpdateCategoryCommand;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Infrastructure.Data;
using BackendAuthTemplate.Tests.Common.Categories;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Categories.Commands
{
    public class UpdateCategoryCommandFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task UpdateCategoryCommand_WithValidData_ShouldSucceed()
        {
            AppDbContext context = _fixture.ServiceProvider.GetRequiredService<AppDbContext>();
            Category category = await CategoriesEntitiesTestHelper.SeedCategory(context);

            UpdateCategoryCommand updateCommand = CategoriesCommandsTestHelper.UpdateCategoryCommand(id: category.Id);

            Result updateResult = await _mediator.Send(updateCommand);

            updateResult.Succeeded.ShouldBeTrue();
            (await context.Categories.FindAsync(updateCommand.CategoryId)).ShouldNotBeNull().Name.ShouldBe(updateCommand.Name);
        }

        [Fact]
        public async Task UpdateCategoryCommand_WithInvalidId_ShouldFail()
        {
            UpdateCategoryCommand updateCommand = CategoriesCommandsTestHelper.UpdateCategoryCommand();

            Result updateResult = await _mediator.Send(updateCommand);

            updateResult.Succeeded.ShouldBeFalse();
        }
    }
}
