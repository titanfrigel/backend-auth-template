using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.Auth.Commands.ResetPasswordCommand;
using BackendAuthTemplate.Tests.Common.Auth;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Auth.Validators
{
    public class ResetPasswordCommandValidatorFunctionalTests
    {
        private readonly ResetPasswordCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            ResetPasswordCommand command = AuthCommandsTestHelper.ResetPasswordCommand(userId: Guid.Empty);

            TestValidationResult<ResetPasswordCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.UserId)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_Token_Is_Empty()
        {
            ResetPasswordCommand command = AuthCommandsTestHelper.ResetPasswordCommand(token: "");

            TestValidationResult<ResetPasswordCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Token)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_NewPassword_Is_TooShort()
        {
            ResetPasswordCommand command = AuthCommandsTestHelper.ResetPasswordCommand(newPassword: "short");

            TestValidationResult<ResetPasswordCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.NewPassword)
                  .WithErrorCode(ValidationCodes.TooShort.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Command()
        {
            ResetPasswordCommand command = AuthCommandsTestHelper.ResetPasswordCommand();

            TestValidationResult<ResetPasswordCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
