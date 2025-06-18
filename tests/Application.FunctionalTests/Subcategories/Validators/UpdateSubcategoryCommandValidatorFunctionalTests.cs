using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.Subcategories.Commands.UpdateSubcategoryCommand;
using BackendAuthTemplate.Tests.Common.Subcategories;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Subcategories.Validators
{
    public class UpdateSubcategoryCommandValidatorFunctionalTests
    {
        private readonly UpdateSubcategoryCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_SubcategoryId_Is_Empty()
        {
            UpdateSubcategoryCommand command = SubcategoriesCommandsTestHelper.UpdateSubcategoryCommand(id: Guid.Empty);

            TestValidationResult<UpdateSubcategoryCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.SubcategoryId)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_TooShort()
        {
            UpdateSubcategoryCommand command = SubcategoriesCommandsTestHelper.UpdateSubcategoryCommand(name: "ab");

            TestValidationResult<UpdateSubcategoryCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorCode(ValidationCodes.TooShort.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_CategoryId_Is_Empty()
        {
            UpdateSubcategoryCommand command = SubcategoriesCommandsTestHelper.UpdateSubcategoryCommand(categoryId: Guid.Empty);

            TestValidationResult<UpdateSubcategoryCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.CategoryId)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Command()
        {
            UpdateSubcategoryCommand command = SubcategoriesCommandsTestHelper.UpdateSubcategoryCommand();

            TestValidationResult<UpdateSubcategoryCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
