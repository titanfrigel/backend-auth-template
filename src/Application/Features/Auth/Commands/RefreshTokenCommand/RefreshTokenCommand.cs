using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Auth.Dtos;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.RefreshTokenCommand
{
    public class RefreshTokenCommand : IRequest<Result<ReadTokenDto>>
    {
    }
}
