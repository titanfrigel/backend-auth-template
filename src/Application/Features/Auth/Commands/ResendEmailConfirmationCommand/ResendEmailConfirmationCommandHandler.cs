using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.ResendEmailConfirmationCommand
{
    public class ResendEmailConfirmationCommandHandler(IIdentityService identityService) : IRequestHandler<ResendEmailConfirmationCommand, Result>
    {
        public async Task<Result> Handle(ResendEmailConfirmationCommand request, CancellationToken cancellationToken = default)
        {
            return await identityService.ResendEmailConfirmationAsync(request, cancellationToken);
        }
    }
}
