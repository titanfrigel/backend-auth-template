using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Users.Queries.GetUserQuery
{
    public class GetUserQuery : IRequest<Result<ReadUserDto>>
    {
        public required Guid UserId { get; init; }
    }
}
