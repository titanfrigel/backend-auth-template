using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.RegisterCommand
{
    public class RegisterCommandHandler(IIdentityService identityService) : IRequestHandler<RegisterCommand, Result>
    {
        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken = default)
        {
            return await identityService.RegisterAsync(request, cancellationToken);
        }
    }
}
