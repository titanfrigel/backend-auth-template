using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Users.Queries.GetUserMeQuery
{
    public class GetUserMeQuery : IRequest<Result<ReadUserDto>>
    {
    }
}
