using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.Subcategories.Commands.DeleteSubcategoryCommand;
using BackendAuthTemplate.Tests.Common.Subcategories;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Subcategories.Validators
{
    public class DeleteSubcategoryCommandValidatorFunctionalTests
    {
        private readonly DeleteSubcategoryCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_SubcategoryId_Is_Empty()
        {
            DeleteSubcategoryCommand command = SubcategoriesCommandsTestHelper.DeleteSubcategoryCommand(id: Guid.Empty);

            TestValidationResult<DeleteSubcategoryCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.SubcategoryId)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Command()
        {
            DeleteSubcategoryCommand command = SubcategoriesCommandsTestHelper.DeleteSubcategoryCommand();

            TestValidationResult<DeleteSubcategoryCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
