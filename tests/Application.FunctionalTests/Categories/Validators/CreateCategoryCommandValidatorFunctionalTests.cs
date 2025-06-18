using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.Categories.Commands.CreateCategoryCommand;
using BackendAuthTemplate.Tests.Common.Categories;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Categories.Validators
{
    public class CreateCategoryCommandValidatorFunctionalTests
    {
        private readonly CreateCategoryCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            CreateCategoryCommand command = CategoriesCommandsTestHelper.CreateCategoryCommand(name: "");

            TestValidationResult<CreateCategoryCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Command()
        {
            CreateCategoryCommand command = CategoriesCommandsTestHelper.CreateCategoryCommand();

            TestValidationResult<CreateCategoryCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
