using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.Auth.Commands.RegisterCommand;
using BackendAuthTemplate.Tests.Common.Auth;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Auth.Validators
{
    public class RegisterCommandValidatorFunctionalTests
    {
        private readonly RegisterCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Email_Is_Empty()
        {
            RegisterCommand command = AuthCommandsTestHelper.RegisterCommand(email: "");

            TestValidationResult<RegisterCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            RegisterCommand command = AuthCommandsTestHelper.RegisterCommand(email: "invalid-email");

            TestValidationResult<RegisterCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorCode(ValidationCodes.Invalid.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Empty()
        {
            RegisterCommand command = AuthCommandsTestHelper.RegisterCommand(password: "");

            TestValidationResult<RegisterCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Password)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_TooShort()
        {
            RegisterCommand command = AuthCommandsTestHelper.RegisterCommand(password: "short");

            TestValidationResult<RegisterCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Password)
                  .WithErrorCode(ValidationCodes.TooShort.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_Password_DoesNotContainDigit()
        {
            RegisterCommand command = AuthCommandsTestHelper.RegisterCommand(password: "PasswordNoDigit");

            TestValidationResult<RegisterCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Password)
                  .WithErrorCode(ValidationCodes.Invalid.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_IsEmpty()
        {
            RegisterCommand command = AuthCommandsTestHelper.RegisterCommand(firstName: "");

            TestValidationResult<RegisterCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.FirstName)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_When_FirstName_ContainsAccents()
        {
            RegisterCommand command = AuthCommandsTestHelper.RegisterCommand(firstName: "NömÄâccëntéàè");

            TestValidationResult<RegisterCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Have_Error_When_LastName_IsEmpty()
        {
            RegisterCommand command = AuthCommandsTestHelper.RegisterCommand(lastName: "");

            TestValidationResult<RegisterCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.LastName)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_When_LastName_ContainsAccents()
        {
            RegisterCommand command = AuthCommandsTestHelper.RegisterCommand(lastName: "NömÄâccëntéàè");

            TestValidationResult<RegisterCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Have_Error_When_PhoneNumber_IsInvalid()
        {
            RegisterCommand command = AuthCommandsTestHelper.RegisterCommand(phoneNumber: "InvalidPhone");

            TestValidationResult<RegisterCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                  .WithErrorCode(ValidationCodes.Invalid.ToString());
        }


        [Fact]
        public void Should_Have_Error_When_Address_IsEmpty()
        {
            RegisterCommand command = AuthCommandsTestHelper.RegisterCommand(address: "");

            TestValidationResult<RegisterCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Address)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_City_IsEmpty()
        {
            RegisterCommand command = AuthCommandsTestHelper.RegisterCommand(city: "");

            TestValidationResult<RegisterCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.City)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_ZipCode_IsInvalid()
        {
            RegisterCommand command = AuthCommandsTestHelper.RegisterCommand(zipCode: "InvalidZipCode#");

            TestValidationResult<RegisterCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.ZipCode)
                  .WithErrorCode(ValidationCodes.Invalid.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_CountryCode_IsInvalid()
        {
            RegisterCommand command = AuthCommandsTestHelper.RegisterCommand(countryCode: "InvalidCountry");

            TestValidationResult<RegisterCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.CountryCode)
                  .WithErrorCode(ValidationCodes.Invalid.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Command()
        {
            RegisterCommand command = AuthCommandsTestHelper.RegisterCommand();

            TestValidationResult<RegisterCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
