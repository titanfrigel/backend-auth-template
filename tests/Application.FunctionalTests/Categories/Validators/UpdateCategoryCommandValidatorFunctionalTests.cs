using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.Categories.Commands.UpdateCategoryCommand;
using BackendAuthTemplate.Tests.Common.Categories;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Categories.Validators
{
    public class UpdateCategoryCommandValidatorFunctionalTests
    {
        private readonly UpdateCategoryCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_CategoryId_Is_Empty()
        {
            UpdateCategoryCommand command = CategoriesCommandsTestHelper.UpdateCategoryCommand(id: Guid.Empty);

            TestValidationResult<UpdateCategoryCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.CategoryId)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_TooShort()
        {
            UpdateCategoryCommand command = CategoriesCommandsTestHelper.UpdateCategoryCommand(name: "ab");

            TestValidationResult<UpdateCategoryCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorCode(ValidationCodes.TooShort.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Command()
        {
            UpdateCategoryCommand command = CategoriesCommandsTestHelper.UpdateCategoryCommand();

            TestValidationResult<UpdateCategoryCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
