using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Auth.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.LoginCommand
{
    public class LoginCommand : IRequest<Result<ReadTokenDto>>
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required bool RememberMe { get; init; }
    }
}
