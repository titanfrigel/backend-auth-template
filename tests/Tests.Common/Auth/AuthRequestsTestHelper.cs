using BackendAuthTemplate.API.Requests.Auth;

namespace BackendAuthTemplate.Tests.Common.Auth
{
    public class AuthRequestsTestHelper
    {
        public static ConfirmEmailRequest ConfirmEmailRequest(
            Guid? userId = null,
            string token = "FakeToken"
        )
        {
            return new()
            {
                UserId = userId ?? Guid.NewGuid(),
                Token = token
            };
        }

        public static ForgotPasswordRequest ForgotPasswordRequest(
            string email = "FakeEmail@example.com"
        )
        {
            return new()
            {
                Email = email
            };
        }

        public static LoginRequest LoginRequest(
            string email = "admin@example.com",
            string password = "AdminPass123!",
            bool rememberMe = false
        )
        {
            return new()
            {
                Email = email,
                Password = password,
                RememberMe = rememberMe
            };
        }

        public static RegisterRequest RegisterRequest(
            string email = "fake.email@example.com",
            string password = "Password1234",
            string phoneNumber = "+33666666666",
            string firstName = "John",
            string lastName = "Doe",
            string address = "123 Fake Street",
            string city = "Faketown",
            string zipCode = "12345",
            string countryCode = "FRA"
        )
        {
            return new()
            {
                Email = email,
                Password = password,
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                City = city,
                ZipCode = zipCode,
                CountryCode = countryCode
            };
        }
        public static ResendEmailConfirmationRequest ResendEmailConfirmationRequest(
            string email = "FakeEmail@example.com"
        )
        {
            return new()
            {
                Email = email
            };
        }

        public static ResetPasswordRequest ResetPasswordRequest(
            Guid? userId = null,
            string token = "FakeToken",
            string newPassword = "NewFakePassword"
        )
        {
            return new()
            {
                UserId = userId ?? Guid.NewGuid(),
                Token = token,
                NewPassword = newPassword
            };
        }


    }
}
