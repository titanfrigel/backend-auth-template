using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.ResendEmailConfirmationCommand
{
    public class ResendEmailConfirmationCommand : IRequest<Result>
    {
        public required string Email { get; init; }
    }
}
