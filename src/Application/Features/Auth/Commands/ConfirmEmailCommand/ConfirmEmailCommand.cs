using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.ConfirmEmailCommand
{
    public class ConfirmEmailCommand : IRequest<Result>
    {
        public required Guid UserId { get; init; }
        public required string Token { get; init; }
    }
}
