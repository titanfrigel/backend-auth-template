using BackendAuthTemplate.Application.Common.Validation;
using BackendAuthTemplate.Application.Features.Users.Commands.UpdateUserMeCommand;
using BackendAuthTemplate.Tests.Common.Users;
using FluentValidation.TestHelper;

namespace BackendAuthTemplate.Application.FunctionalTests.Users.Validators
{
    public class UpdateUserMeCommandValidatorFunctionalTests
    {
        private readonly UpdateUserMeCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_FirstName_IsEmpty()
        {
            UpdateUserMeCommand command = UsersCommandsTestHelper.UpdateUserMeCommand(firstName: "");

            TestValidationResult<UpdateUserMeCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.FirstName)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_When_FirstName_ContainsAccents()
        {
            UpdateUserMeCommand command = UsersCommandsTestHelper.UpdateUserMeCommand(firstName: "NömÄâccëntéàè");

            TestValidationResult<UpdateUserMeCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Have_Error_When_LastName_IsEmpty()
        {
            UpdateUserMeCommand command = UsersCommandsTestHelper.UpdateUserMeCommand(lastName: "");

            TestValidationResult<UpdateUserMeCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.LastName)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_When_LastName_ContainsAccents()
        {
            UpdateUserMeCommand command = UsersCommandsTestHelper.UpdateUserMeCommand(lastName: "NömÄâccëntéàè");

            TestValidationResult<UpdateUserMeCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Have_Error_When_PhoneNumber_IsInvalid()
        {
            UpdateUserMeCommand command = UsersCommandsTestHelper.UpdateUserMeCommand(phoneNumber: "InvalidPhone");

            TestValidationResult<UpdateUserMeCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                  .WithErrorCode(ValidationCodes.Invalid.ToString());
        }


        [Fact]
        public void Should_Have_Error_When_Address_IsEmpty()
        {
            UpdateUserMeCommand command = UsersCommandsTestHelper.UpdateUserMeCommand(address: "");

            TestValidationResult<UpdateUserMeCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.Address)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_City_IsEmpty()
        {
            UpdateUserMeCommand command = UsersCommandsTestHelper.UpdateUserMeCommand(city: "");

            TestValidationResult<UpdateUserMeCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.City)
                  .WithErrorCode(ValidationCodes.Required.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_ZipCode_IsInvalid()
        {
            UpdateUserMeCommand command = UsersCommandsTestHelper.UpdateUserMeCommand(zipCode: "InvalidZipCode#");

            TestValidationResult<UpdateUserMeCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.ZipCode)
                  .WithErrorCode(ValidationCodes.Invalid.ToString());
        }

        [Fact]
        public void Should_Have_Error_When_CountryCode_IsInvalid()
        {
            UpdateUserMeCommand command = UsersCommandsTestHelper.UpdateUserMeCommand(countryCode: "InvalidCountry");

            TestValidationResult<UpdateUserMeCommand> result = _validator.TestValidate(command);

            _ = result.ShouldHaveValidationErrorFor(x => x.CountryCode)
                  .WithErrorCode(ValidationCodes.Invalid.ToString());
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Command()
        {
            UpdateUserMeCommand command = UsersCommandsTestHelper.UpdateUserMeCommand();

            TestValidationResult<UpdateUserMeCommand> result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
