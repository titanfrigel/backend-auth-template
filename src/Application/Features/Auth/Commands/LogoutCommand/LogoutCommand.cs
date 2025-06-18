using BackendAuthTemplate.Application.Common.Result;
using MediatR;

namespace BackendAuthTemplate.Application.Features.Auth.Commands.LogoutCommand
{
    public class LogoutCommand : IRequest<Result>
    {
    }
}
