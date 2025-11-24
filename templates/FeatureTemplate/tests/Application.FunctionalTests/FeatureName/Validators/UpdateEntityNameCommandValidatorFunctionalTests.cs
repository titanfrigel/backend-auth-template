using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.FeatureName.Commands.UpdateEntityNameCommand;
using BackendAuthTemplate.Tests.Common.FeatureName;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.FeatureName.Validators
{
    public class UpdateEntityNameCommandValidatorFunctionalTests
    {
        private readonly UpdateEntityNameCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_EntityNameId_Is_Empty()
        {
            UpdateEntityNameCommand command = FeatureNameCommandsTestHelper.UpdateEntityNameCommand(id: Guid.Empty);

            TestValidationResult<UpdateEntityNameCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.EntityNameId)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_TooShort()
        {
            UpdateEntityNameCommand command = FeatureNameCommandsTestHelper.UpdateEntityNameCommand(name: "ab");

            TestValidationResult<UpdateEntityNameCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorCode(ValidationCodes.TooShort.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Command()
        {
            UpdateEntityNameCommand command = FeatureNameCommandsTestHelper.UpdateEntityNameCommand();

            TestValidationResult<UpdateEntityNameCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
