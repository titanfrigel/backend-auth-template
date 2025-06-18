using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Subcategories.Commands.DeleteSubcategoryCommand;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Infrastructure.Data;
using BackendAuthTemplate.Tests.Common.Subcategories;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Subcategories.Commands
{
    public class DeleteSubcategoryCommandFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task DeleteSubcategoryCommand_WithValidId_ShouldSucceed()
        {
            AppDbContext context = _fixture.ServiceProvider.GetRequiredService<AppDbContext>();
            Subcategory subcategory = await SubcategoriesEntitiesTestHelper.SeedSubcategory(context);

            DeleteSubcategoryCommand deleteCommand = SubcategoriesCommandsTestHelper.DeleteSubcategoryCommand(id: subcategory.Id);

            Result deleteResult = await _mediator.Send(deleteCommand);

            deleteResult.Succeeded.ShouldBeTrue();
            (await context.Subcategories.FindAsync(deleteCommand.SubcategoryId)).ShouldBeNull();
        }

        [Fact]
        public async Task DeleteSubcategoryCommand_WithInvalidId_ShouldFail()
        {
            DeleteSubcategoryCommand deleteCommand = SubcategoriesCommandsTestHelper.DeleteSubcategoryCommand();

            Result deleteResult = await _mediator.Send(deleteCommand);

            deleteResult.Succeeded.ShouldBeFalse();
        }
    }
}
