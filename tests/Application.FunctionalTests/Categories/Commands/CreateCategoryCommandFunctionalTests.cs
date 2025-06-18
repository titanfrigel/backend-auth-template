using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Commands.CreateCategoryCommand;
using BackendAuthTemplate.Tests.Common.Categories;
using Shouldly;

namespace BackendAuthTemplate.Application.FunctionalTests.Categories.Commands
{
    public class CreateCategoryCommandFunctionalTests : ApplicationTestBase
    {
        [Fact]
        public async Task CreateCategoryCommand_WithValidName_ShouldReturnCategoryId()
        {
            CreateCategoryCommand command = CategoriesCommandsTestHelper.CreateCategoryCommand();

            Result<Guid> result = await _mediator.Send(command);

            result.Succeeded.ShouldBeTrue();
            result.Value.ShouldNotBe(Guid.Empty);
        }
    }
}
