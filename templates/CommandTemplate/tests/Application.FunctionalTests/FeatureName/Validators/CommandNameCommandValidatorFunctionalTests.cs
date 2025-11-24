using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.FeatureName.Commands.CommandNameCommand;
using BackendAuthTemplate.Tests.Common.FeatureName;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.FeatureName.Validators
{
    public class CommandNameCommandValidatorFunctionalTests
    {
        private readonly CommandNameCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            CommandNameCommand command = FeatureNameCommandsTestHelper.CommandNameCommand(name: "");

            TestValidationResult<CommandNameCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Command()
        {
            CommandNameCommand command = FeatureNameCommandsTestHelper.CommandNameCommand();

            TestValidationResult<CommandNameCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
