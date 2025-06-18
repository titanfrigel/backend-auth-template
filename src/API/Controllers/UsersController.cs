using Asp.Versioning;
using AutoMapper;
using BackendAuthTemplate.API.Common.Extensions;
using BackendAuthTemplate.API.Requests.Users;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Users.Commands.UpdateUserMeCommand;
using BackendAuthTemplate.Application.Features.Users.Dtos;
using BackendAuthTemplate.Application.Features.Users.Queries.GetUserMeQuery;
using BackendAuthTemplate.Application.Features.Users.Queries.GetUserQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendAuthTemplate.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{v:apiVersion}/users")]
    [Authorize(Roles = "User")]
    public class UsersController(IMediator mediator, IMapper mapper) : BaseController
    {
        [HttpGet("me")]
        public async Task<ActionResult<ReadUserDto>> GetUserMe()
        {
            GetUserMeQuery query = new();

            Result<ReadUserDto> result = await mediator.Send(query);

            return result.Match(
                onSuccess: Ok,
                onFailure: Problem
            );
        }

        [HttpPut("me")]
        public async Task<ActionResult> UpdateUserMe([FromBody] UpdateUserMeRequest request)
        {
            UpdateUserMeCommand command = mapper.Map<UpdateUserMeCommand>(request);

            Result result = await mediator.Send(command);

            return result.Match(
                onSuccess: NoContent,
                onFailure: Problem
            );
        }

        [HttpGet("{userId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ReadUserDto>> GetUser([FromRoute] Guid userId)
        {
            GetUserQuery query = new()
            {
                UserId = userId
            };

            Result<ReadUserDto> result = await mediator.Send(query);

            return result.Match(
                onSuccess: Ok,
                onFailure: Problem
            );
        }
    }
}
