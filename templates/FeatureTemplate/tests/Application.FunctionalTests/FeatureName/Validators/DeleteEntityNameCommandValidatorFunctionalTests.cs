using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.FeatureName.Commands.DeleteEntityNameCommand;
using BackendAuthTemplate.Tests.Common.FeatureName;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.FeatureName.Validators
{
    public class DeleteEntityNameCommandValidatorFunctionalTests
    {
        private readonly DeleteEntityNameCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_EntityNameId_Is_Empty()
        {
            DeleteEntityNameCommand command = FeatureNameCommandsTestHelper.DeleteEntityNameCommand(id: Guid.Empty);

            TestValidationResult<DeleteEntityNameCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.EntityNameId)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Command()
        {
            DeleteEntityNameCommand command = FeatureNameCommandsTestHelper.DeleteEntityNameCommand();

            TestValidationResult<DeleteEntityNameCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
