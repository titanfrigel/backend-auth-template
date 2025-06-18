using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.ResetPasswordCommand
{
    public class ResetPasswordCommandHandler(IIdentityService identityService) : IRequestHandler<ResetPasswordCommand, Result>
    {
        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken = default)
        {
            return await identityService.ResetPasswordAsync(request, cancellationToken);
        }
    }
}
