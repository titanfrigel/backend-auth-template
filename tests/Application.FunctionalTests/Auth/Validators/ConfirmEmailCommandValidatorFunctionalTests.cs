using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.Auth.Commands.ConfirmEmailCommand;
using BackendAuthTemplate.Tests.Common.Auth;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Auth.Validators
{
    public class ConfirmEmailCommandValidatorFunctionalTests
    {
        private readonly ConfirmEmailCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            ConfirmEmailCommand command = AuthCommandsTestHelper.ConfirmEmailCommand(userId: Guid.Empty);

            TestValidationResult<ConfirmEmailCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.UserId)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_Token_Is_Empty()
        {
            ConfirmEmailCommand command = AuthCommandsTestHelper.ConfirmEmailCommand(token: "");

            TestValidationResult<ConfirmEmailCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Token)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Command()
        {
            ConfirmEmailCommand command = AuthCommandsTestHelper.ConfirmEmailCommand();

            TestValidationResult<ConfirmEmailCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
