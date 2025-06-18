using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.Auth.Commands.ForgotPasswordCommand;
using BackendAuthTemplate.Tests.Common.Auth;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Auth.Validators
{
    public class ForgotPasswordCommandValidatorFunctionalTests
    {
        private readonly ForgotPasswordCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Email_Is_Empty()
        {
            ForgotPasswordCommand command = AuthCommandsTestHelper.ForgotPasswordCommand(email: "");

            TestValidationResult<ForgotPasswordCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            ForgotPasswordCommand command = AuthCommandsTestHelper.ForgotPasswordCommand(email: "invalid-email");

            TestValidationResult<ForgotPasswordCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorCode(ValidationCodes.Invalid.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Command()
        {
            ForgotPasswordCommand command = AuthCommandsTestHelper.ForgotPasswordCommand();

            TestValidationResult<ForgotPasswordCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
