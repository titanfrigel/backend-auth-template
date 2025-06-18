using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Auth.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.LoginCommand
{
    public class LoginCommandHandler(IIdentityService identityService) : IRequestHandler<LoginCommand, Result<ReadTokenDto>>
    {
        public async Task<Result<ReadTokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken = default)
        {
            return await identityService.LoginAsync(request, cancellationToken);
        }
    }
}
