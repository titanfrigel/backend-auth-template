using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.LogoutCommand
{
    public class LogoutCommandHandler(IIdentityService identityService) : IRequestHandler<LogoutCommand, Result>
    {
        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken = default)
        {
            return await identityService.LogoutAsync(request, cancellationToken);
        }
    }
}
