using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Auth.Commands.ConfirmEmailCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.ForgotPasswordCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.LoginCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.LogoutCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.RefreshTokenCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.RegisterCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.ResendEmailConfirmationCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.ResetPasswordCommand;
using BackendAuthTemplate.Application.Features.Auth.Dtos;
using BackendAuthTemplate.Application.Features.Users.Commands.UpdateUserMeCommand;
using BackendAuthTemplate.Application.Features.Users.Dtos;

namespace BackendAuthTemplate.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Result.Result> RegisterAsync(RegisterCommand command, CancellationToken cancellationToken = default);
        Task<Result.Result> ConfirmEmailAsync(ConfirmEmailCommand command, CancellationToken cancellationToken = default);
        Task<Result.Result> ResendEmailConfirmationAsync(ResendEmailConfirmationCommand command, CancellationToken cancellationToken = default);
        Task<Result.Result> ForgotPasswordAsync(ForgotPasswordCommand command, CancellationToken cancellationToken = default);
        Task<Result.Result> ResetPasswordAsync(ResetPasswordCommand command, CancellationToken cancellationToken = default);
        Task<Result<ReadTokenDto>> LoginAsync(LoginCommand command, CancellationToken cancellationToken = default);
        Task<Result.Result> LogoutAsync(LogoutCommand command, CancellationToken cancellationToken = default);
        Task<Result<ReadTokenDto>> RefreshTokenAsync(RefreshTokenCommand command, CancellationToken cancellationToken = default);
        Task<Result.Result> UpdateUserMeAsync(UpdateUserMeCommand command, CancellationToken cancellationToken = default);
        Task<ReadUserDto?> GetUserMeAsync(CancellationToken cancellationToken = default);
        Task<ReadUserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
