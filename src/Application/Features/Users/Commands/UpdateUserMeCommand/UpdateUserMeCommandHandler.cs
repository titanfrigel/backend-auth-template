using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Users.Commands.UpdateUserMeCommand
{
    public class UpdateUserMeCommandCommandHandler(IIdentityService identityService) : IRequestHandler<UpdateUserMeCommand, Result>
    {
        public async Task<Result> Handle(UpdateUserMeCommand request, CancellationToken cancellationToken = default)
        {
            return await identityService.UpdateUserMeAsync(request, cancellationToken);
        }
    }
}
