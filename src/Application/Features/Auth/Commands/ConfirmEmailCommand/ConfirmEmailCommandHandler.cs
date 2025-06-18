using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.ConfirmEmailCommand
{
    public class ConfirmEmailCommandHandler(IIdentityService identityService) : IRequestHandler<ConfirmEmailCommand, Result>
    {
        public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken = default)
        {
            return await identityService.ConfirmEmailAsync(request, cancellationToken);
        }
    }
}
