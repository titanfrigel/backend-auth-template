using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.Subcategories.Commands.CreateSubcategoryCommand;
using BackendAuthTemplate.Tests.Common.Subcategories;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Subcategories.Validators
{
    public class CreateSubcategoryCommandValidatorFunctionalTests
    {
        private readonly CreateSubcategoryCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            CreateSubcategoryCommand command = SubcategoriesCommandsTestHelper.CreateSubcategoryCommand(name: "");

            TestValidationResult<CreateSubcategoryCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_CategoryId_Is_Empty()
        {
            CreateSubcategoryCommand command = SubcategoriesCommandsTestHelper.CreateSubcategoryCommand(categoryId: Guid.Empty);

            TestValidationResult<CreateSubcategoryCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.CategoryId)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Command()
        {
            CreateSubcategoryCommand command = SubcategoriesCommandsTestHelper.CreateSubcategoryCommand();

            TestValidationResult<CreateSubcategoryCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
