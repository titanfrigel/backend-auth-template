using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Commands.DeleteCategoryCommand;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Infrastructure.Data;
using BackendAuthTemplate.Tests.Common.Categories;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Categories.Commands
{
    public class DeleteCategoryCommandFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task DeleteCategoryCommand_WithValidId_ShouldSucceed()
        {
            AppDbContext context = _fixture.ServiceProvider.GetRequiredService<AppDbContext>();
            Category category = await CategoriesEntitiesTestHelper.SeedCategory(context);

            DeleteCategoryCommand deleteCommand = CategoriesCommandsTestHelper.DeleteCategoryCommand(category.Id);

            Result deleteResult = await _mediator.Send(deleteCommand);

            deleteResult.Succeeded.ShouldBeTrue();
            (await context.Categories.FindAsync(deleteCommand.CategoryId)).ShouldBeNull();
        }

        [Fact]
        public async Task DeleteCategoryCommand_WithInvalidId_ShouldFail()
        {
            DeleteCategoryCommand deleteCommand = CategoriesCommandsTestHelper.DeleteCategoryCommand();

            Result deleteResult = await _mediator.Send(deleteCommand);

            deleteResult.Succeeded.ShouldBeFalse();
        }
    }
}
