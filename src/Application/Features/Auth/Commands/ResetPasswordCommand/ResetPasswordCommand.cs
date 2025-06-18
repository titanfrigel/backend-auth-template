using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.ResetPasswordCommand
{
    public class ResetPasswordCommand : IRequest<Result>
    {
        public required Guid UserId { get; init; }
        public required string Token { get; init; }
        public required string NewPassword { get; init; }
    }
}
