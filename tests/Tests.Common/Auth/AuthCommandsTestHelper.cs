using BackendAuthTemplate.Application.Features.Auth.Commands.ConfirmEmailCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.ForgotPasswordCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.LoginCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.LogoutCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.RefreshTokenCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.RegisterCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.ResetPasswordCommand;

namespace BackendAuthTemplate.Tests.Common.Auth
{
    public static class AuthCommandsTestHelper
    {
        public static ConfirmEmailCommand ConfirmEmailCommand(
            Guid? userId = null,
            string token = "validtoken"
        )
        {
            return new()
            {
                UserId = userId ?? Guid.NewGuid(),
                Token = token
            };
        }

        public static ForgotPasswordCommand ForgotPasswordCommand(
            string email = "invalidemail@example.com"
        )
        {
            return new()
            {
                Email = email
            };
        }

        public static LoginCommand LoginCommand(
            string email = "admin@example.com",
            string password = "AdminPass123!",
            bool remember = false
        )
        {
            return new()
            {
                Email = email,
                Password = password,
                RememberMe = remember
            };
        }

        public static LogoutCommand LogoutCommand()
        {
            return new()
            {
            };
        }

        public static RefreshTokenCommand RefreshTokenCommand(string? refreshToken = null)
        {
            return new()
            {
                RefreshToken = refreshToken
            };
        }

        public static RegisterCommand RegisterCommand(
            string email = "validemail@example.com",
            string password = "Password1234",
            string phoneNumber = "+33666666666",
            string firstName = "Tom",
            string lastName = "Etnana",
            string address = "12 Rue de la Rue",
            string city = "Nancy",
            string zipCode = "54000",
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

        public static ResetPasswordCommand ResetPasswordCommand(
            Guid? userId = null,
            string token = "validtoken",
            string newPassword = "Password123"
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
