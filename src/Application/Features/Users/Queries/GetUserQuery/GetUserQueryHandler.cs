using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Users.Queries.GetUserQuery
{
    public class GetUserQueryHandler(IIdentityService identityService) : IRequestHandler<GetUserQuery, Result<ReadUserDto>>
    {
        public async Task<Result<ReadUserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken = default)
        {
            ReadUserDto? appUser = await identityService.GetUserByIdAsync(request.UserId, cancellationToken);

            if (appUser == null)
            {
                return UsersErrors.NotFound(request.UserId);
            }

            return appUser;
        }
    }
}
