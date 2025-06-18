using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Users.Queries.GetUserMeQuery
{
    public class GetUserMeQueryHandler(IIdentityService identityService) : IRequestHandler<GetUserMeQuery, Result<ReadUserDto>>
    {
        public async Task<Result<ReadUserDto>> Handle(GetUserMeQuery request, CancellationToken cancellationToken = default)
        {
            ReadUserDto? appUser = await identityService.GetUserMeAsync(cancellationToken);

            if (appUser == null)
            {
                return UsersErrors.NotFound(Guid.Empty);
            }

            return appUser;
        }
    }
}
