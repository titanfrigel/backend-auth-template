using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.Categories.Commands.DeleteCategoryCommand;
using BackendAuthTemplate.Tests.Common.Categories;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Categories.Validators
{
    public class DeleteCategoryCommandValidatorFunctionalTests
    {
        private readonly DeleteCategoryCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_CategoryId_Is_Empty()
        {
            DeleteCategoryCommand command = CategoriesCommandsTestHelper.DeleteCategoryCommand(id: Guid.Empty);

            TestValidationResult<DeleteCategoryCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.CategoryId)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Command()
        {
            DeleteCategoryCommand command = CategoriesCommandsTestHelper.DeleteCategoryCommand();

            TestValidationResult<DeleteCategoryCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
