using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.FeatureName.Commands.CreateEntityNameCommand;
using BackendAuthTemplate.Tests.Common.FeatureName;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.FeatureName.Validators
{
    public class CreateEntityNameCommandValidatorFunctionalTests
    {
        private readonly CreateEntityNameCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            CreateEntityNameCommand command = FeatureNameCommandsTestHelper.CreateEntityNameCommand(name: "");

            TestValidationResult<CreateEntityNameCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Command()
        {
            CreateEntityNameCommand command = FeatureNameCommandsTestHelper.CreateEntityNameCommand();

            TestValidationResult<CreateEntityNameCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
