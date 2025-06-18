using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Auth.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.RefreshTokenCommand
{
    public class RefreshTokenCommandHandler(IIdentityService identityService) : IRequestHandler<RefreshTokenCommand, Result<ReadTokenDto>>
    {
        public async Task<Result<ReadTokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken = default)
        {
            return await identityService.RefreshTokenAsync(request, cancellationToken);
        }
    }
}
