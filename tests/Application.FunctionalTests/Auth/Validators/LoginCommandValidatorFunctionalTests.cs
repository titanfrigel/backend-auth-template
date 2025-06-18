using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.Auth.Commands.LoginCommand;
using BackendAuthTemplate.Tests.Common.Auth;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Auth.Validators
{
    public class LoginCommandValidatorFunctionalTests
    {
        private readonly LoginCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Email_Is_Empty()
        {
            LoginCommand command = AuthCommandsTestHelper.LoginCommand(email: "");

            TestValidationResult<LoginCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            LoginCommand command = AuthCommandsTestHelper.LoginCommand(email: "invalid-email");

            TestValidationResult<LoginCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorCode(ValidationCodes.Invalid.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Empty()
        {
            LoginCommand command = AuthCommandsTestHelper.LoginCommand(password: "");

            TestValidationResult<LoginCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Password)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Command()
        {
            LoginCommand command = AuthCommandsTestHelper.LoginCommand();

            TestValidationResult<LoginCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
