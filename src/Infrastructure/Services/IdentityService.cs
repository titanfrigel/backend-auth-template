using AutoMapper;
using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Common.Settings;
using BackendAuthTemplate.Application.Features.Auth.Commands.ConfirmEmailCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.ForgotPasswordCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.LoginCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.LogoutCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.RefreshTokenCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.RegisterCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.ResendEmailConfirmationCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.ResetPasswordCommand;
using BackendAuthTemplate.Application.Features.Auth.Dtos;
using BackendAuthTemplate.Application.Features.Users;
using BackendAuthTemplate.Application.Features.Users.Commands.UpdateUserMeCommand;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using BackendAuthTemplate.Domain.Interfaces;
using BackendAuthTemplate.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using BackendAuthTemplate.Application.Common.Utils;

namespace BackendAuthTemplate.Infrastructure.Services
{
    public class IdentityService(
        UserManager<AppUser> userManager,
        IEmailService emailService,
        ICookieService cookieService,
        IUser userContext,
        ITokenService tokenService,
        IOptions<SecuritySettings> securityOptions,
        IOptions<GeneralSettings> generalOptions,
        IOptions<AuthSettings> jwtOptions,
        TimeProvider timeProvider,
        IMapper mapper
    ) : IIdentityService
    {
        private readonly SecuritySettings securitySettings = securityOptions.Value;
        private readonly GeneralSettings generalSettings = generalOptions.Value;
        private readonly AuthSettings jwtSettings = jwtOptions.Value;

        #region HELPER FUNCTIONS
        private Task SendConfirmEmailAsync(IAppUser user, string token, CancellationToken cancellationToken = default)
        {
            string confirmLink = $"{generalSettings.FrontendUri}/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            return emailService.SendConfirmEmailAsync(user, confirmLink, cancellationToken);
        }

        private Task SendResetPasswordEmailAsync(IAppUser user, string token, CancellationToken cancellationToken = default)
        {
            string resetLink = $"{generalSettings.FrontendUri}/reset-password?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            return emailService.SendResetPasswordEmailAsync(user, resetLink, cancellationToken);
        }

        #endregion

        public async Task<Result> RegisterAsync(RegisterCommand command, CancellationToken cancellationToken = default)
        {
            AppUser? existingUser = await userManager.FindByEmailAsync(command.Email);
            if (existingUser != null)
            {
                return UsersErrors.Conflict("email");
            }

            AppUser user = new()
            {
                UserName = command.Email,
                Email = command.Email,
                FirstName = command.FirstName.CapitalizeProperName(),
                LastName = command.LastName.CapitalizeProperName(),
                PhoneNumber = command.PhoneNumber,
                Address = command.Address,
                City = command.City,
                ZipCode = command.ZipCode,
                CountryCode = command.CountryCode
            };

            IdentityResult createResult = await userManager.CreateAsync(user, command.Password);

            if (!createResult.Succeeded)
            {
                return UsersErrors.Failure();
            }

            IdentityResult addToRoleResult = await userManager.AddToRoleAsync(user, "User");

            if (!addToRoleResult.Succeeded)
            {
                return UsersErrors.Failure();
            }

            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            await SendConfirmEmailAsync(user, token, cancellationToken);

            user.LastVerificationEmailSent = timeProvider.GetUtcNow();
            IdentityResult updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return UsersErrors.Failure();
            }

            return Result.Success();
        }

        public async Task<Result> ConfirmEmailAsync(ConfirmEmailCommand command, CancellationToken cancellationToken = default)
        {
            AppUser? user = await userManager.FindByIdAsync(command.UserId.ToString());
            if (user == null)
            {
                return UsersErrors.Failure();
            }

            if (user.EmailConfirmed)
            {
                return UsersErrors.Failure();
            }

            IdentityResult result = await userManager.ConfirmEmailAsync(user, command.Token);
            if (!result.Succeeded)
            {
                return UsersErrors.Failure();
            }

            return Result.Success();
        }

        public async Task<Result> ResendEmailConfirmationAsync(ResendEmailConfirmationCommand command, CancellationToken cancellationToken = default)
        {
            AppUser? user = await userManager.FindByEmailAsync(command.Email);
            if (user == null)
            {
                return Result.Success();
            }

            if (user.EmailConfirmed)
            {
                return Result.Success();
            }

            DateTimeOffset utcNow = timeProvider.GetUtcNow();

            if (user.LastVerificationEmailSent.HasValue &&
                (utcNow - user.LastVerificationEmailSent.Value).TotalMinutes < securitySettings.EmailVerificationDelayInMinutes)
            {
                return UsersErrors.Failure();
            }

            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            await SendConfirmEmailAsync(user, token, cancellationToken);

            user.LastVerificationEmailSent = utcNow;

            IdentityResult result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return UsersErrors.Failure();
            }

            return Result.Success();
        }

        public async Task<Result> ForgotPasswordAsync(ForgotPasswordCommand command, CancellationToken cancellationToken = default)
        {
            AppUser? user = await userManager.FindByEmailAsync(command.Email);
            if (user == null)
            {
                return Result.Success();
            }

            DateTimeOffset utcNow = timeProvider.GetUtcNow();

            if (user.LastPasswordResetEmailSent.HasValue
                && (utcNow - user.LastPasswordResetEmailSent.Value).TotalMinutes < securitySettings.ResetPasswordDelayInMinutes)
            {
                return UsersErrors.Failure();
            }

            string token = await userManager.GeneratePasswordResetTokenAsync(user);

            await SendResetPasswordEmailAsync(user, token, cancellationToken);

            user.LastPasswordResetEmailSent = utcNow;

            IdentityResult result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return UsersErrors.Failure();
            }

            return Result.Success();
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordCommand command, CancellationToken cancellationToken = default)
        {
            AppUser? user = await userManager.FindByIdAsync(command.UserId.ToString());
            if (user == null)
            {
                return UsersErrors.Failure();
            }

            IdentityResult resetPasswordResult = await userManager.ResetPasswordAsync(user, command.Token, command.NewPassword);
            if (!resetPasswordResult.Succeeded)
            {
                return UsersErrors.Failure();
            }

            user.RefreshTokenHash = null;
            user.RefreshTokenExpiryTime = null;

            IdentityResult result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return UsersErrors.Failure();
            }

            return Result.Success();
        }

        public async Task<Result<ReadTokenDto>> LoginAsync(LoginCommand command, CancellationToken cancellationToken = default)
        {
            AppUser? user = await userManager.FindByEmailAsync(command.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, command.Password))
            {
                return UsersErrors.Failure();
            }

            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                return UsersErrors.Failure();
            }

            IList<string> roles = await userManager.GetRolesAsync(user);
            string token = tokenService.GenerateAccessToken(user.Id, user.Email!, user.FirstName, user.LastName, roles);
            string refreshToken = tokenService.GenerateRefreshToken();

            int refreshExpiresInDays = command.RememberMe
               ? jwtSettings.RememberMeRefreshExpiresInDays
               : jwtSettings.DefaultRefreshExpiresInDays;

            user.RefreshTokenHash = tokenService.Hash(refreshToken);
            user.RefreshTokenExpiryTime = timeProvider.GetUtcNow().AddDays(refreshExpiresInDays);

            IdentityResult result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return UsersErrors.Failure();
            }

            cookieService.SetRefreshToken(tokenService.GenerateRefreshTokenComposite(user.Id, refreshToken), user.RefreshTokenExpiryTime!.Value);

            return new ReadTokenDto()
            {
                AccessToken = token
            };
        }

        public async Task<Result> LogoutAsync(LogoutCommand command, CancellationToken cancellationToken = default)
        {
            AppUser? user = await userManager.FindByIdAsync(userContext.UserId.ToString());
            if (user != null)
            {
                user.RefreshTokenHash = null;
                user.RefreshTokenExpiryTime = null;

                IdentityResult result = await userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return UsersErrors.Failure();
                }
            }

            cookieService.RemoveRefreshToken();

            return Result.Success();
        }


        public async Task<Result<ReadTokenDto>> RefreshTokenAsync(RefreshTokenCommand command, CancellationToken cancellationToken = default)
        {
            string? refreshTokenComposite = cookieService.GetRefreshToken();

            if (refreshTokenComposite == null)
            {
                return UsersErrors.Failure();
            }

            string[] split = refreshTokenComposite.Split('.', 2);

            if (!Guid.TryParse(split[0], out Guid userId))
            {
                return UsersErrors.Failure();
            }

            AppUser? user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user == null)
            {
                return UsersErrors.Failure();
            }

            if (user.RefreshTokenExpiryTime == null ||
                user.RefreshTokenExpiryTime < DateTimeOffset.UtcNow ||
                user.RefreshTokenHash == null ||
                !tokenService.Verify(split[1], user.RefreshTokenHash)
            )
            {
                return UsersErrors.Failure();
            }

            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                return UsersErrors.Failure();
            }

            IList<string> roles = await userManager.GetRolesAsync(user);
            string newToken = tokenService.GenerateAccessToken(user.Id, user.Email!, user.FirstName, user.LastName, roles);
            string newRefreshToken = tokenService.GenerateRefreshToken();

            user.RefreshTokenHash = tokenService.Hash(newRefreshToken);

            IdentityResult result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return UsersErrors.Failure();
            }

            cookieService.SetRefreshToken(tokenService.GenerateRefreshTokenComposite(user.Id, newRefreshToken), user.RefreshTokenExpiryTime!.Value);

            return new ReadTokenDto()
            {
                AccessToken = newToken
            };
        }

        public async Task<Result> UpdateUserMeAsync(UpdateUserMeCommand command, CancellationToken cancellationToken = default)
        {
            AppUser? user = await userManager.FindByIdAsync(userContext.UserId.ToString());

            if (user == null)
            {
                return UsersErrors.NotFound(userContext.UserId);
            }

            user.FirstName = command.FirstName.CapitalizeProperName();
            user.LastName = command.LastName.CapitalizeProperName();
            user.PhoneNumber = command.PhoneNumber;
            user.Address = command.Address;
            user.City = command.City;
            user.ZipCode = command.ZipCode;
            user.CountryCode = command.CountryCode;

            IdentityResult updateResult = await userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return UsersErrors.Failure();
            }

            return Result.Success();
        }

        public async Task<ReadUserDto?> GetUserMeAsync(CancellationToken cancellationToken = default)
        {
            AppUser? user = await userManager.FindByIdAsync(userContext.UserId.ToString());

            if (user == null)
            {
                return null;
            }

            ReadUserDto userDto = mapper.Map<ReadUserDto>(user);

            return userDto;
        }

        public async Task<ReadUserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            AppUser? user = await userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return null;
            }

            ReadUserDto userDto = mapper.Map<ReadUserDto>(user);

            return userDto;
        }
    }
}