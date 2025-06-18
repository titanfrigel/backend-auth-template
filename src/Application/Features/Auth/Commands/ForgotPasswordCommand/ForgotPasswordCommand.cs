using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.ForgotPasswordCommand
{
    public class ForgotPasswordCommand : IRequest<Result>
    {
        public required string Email { get; init; }
    }
}
