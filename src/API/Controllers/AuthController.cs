using Asp.Versioning;
using AutoMapper;
using BackendAuthTemplate.API.Common.Extensions;
using BackendAuthTemplate.API.Requests.Auth;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Auth.Commands.ConfirmEmailCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.ForgotPasswordCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.LoginCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.LogoutCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.RefreshTokenCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.RegisterCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.ResendEmailConfirmationCommand;
using BackendAuthTemplate.Application.Features.Auth.Commands.ResetPasswordCommand;
using BackendAuthTemplate.Application.Features.Auth.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BackendAuthTemplate.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{v:apiVersion}/auth")]
    [AllowAnonymous]
    public class AuthController(IMediator mediator, IMapper mapper) : BaseController
    {
        [HttpPost("register")]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            RegisterCommand command = mapper.Map<RegisterCommand>(request);

            Result result = await mediator.Send(command);

            return result.Match(
                onSuccess: NoContent,
                onFailure: Problem
            );
        }

        [HttpPost("confirm-email")]
        [EnableRateLimiting("auth")]
        public async Task<ActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            ConfirmEmailCommand command = mapper.Map<ConfirmEmailCommand>(request);

            Result result = await mediator.Send(command);

            return result.Match(
                onSuccess: NoContent,
                onFailure: Problem
            );
        }

        [HttpPost("resend-email-confirmation")]
        [EnableRateLimiting("auth")]
        public async Task<ActionResult> ResendEmailConfirmation([FromBody] ResendEmailConfirmationRequest request)
        {
            ResendEmailConfirmationCommand command = mapper.Map<ResendEmailConfirmationCommand>(request);

            Result result = await mediator.Send(command);

            return result.Match(
                onSuccess: NoContent,
                onFailure: Problem
            );
        }

        [HttpPost("forgot-password")]
        [EnableRateLimiting("auth")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            ForgotPasswordCommand command = mapper.Map<ForgotPasswordCommand>(request);

            Result result = await mediator.Send(command);

            return result.Match(
                onSuccess: NoContent,
                onFailure: Problem
            );
        }

        [HttpPost("reset-password")]
        [EnableRateLimiting("auth")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            ResetPasswordCommand command = mapper.Map<ResetPasswordCommand>(request);

            Result result = await mediator.Send(command);

            return result.Match(
                onSuccess: NoContent,
                onFailure: Problem
            );
        }

        [HttpPost("login")]
        [EnableRateLimiting("auth")]
        public async Task<ActionResult<ReadTokenDto>> Login([FromBody] LoginRequest request)
        {
            LoginCommand command = mapper.Map<LoginCommand>(request);

            Result<ReadTokenDto> result = await mediator.Send(command);

            return result.Match(
                onSuccess: Ok,
                onFailure: Problem
            );
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            LogoutCommand command = new();

            Result result = await mediator.Send(command);

            return result.Match(
                onSuccess: NoContent,
                onFailure: Problem
            );
        }

        [HttpPost("refresh")]
        [EnableRateLimiting("auth")]
        public async Task<ActionResult<ReadTokenDto>> Refresh()
        {
            RefreshTokenCommand command = new();

            Result<ReadTokenDto> result = await mediator.Send(command);

            return result.Match(
                onSuccess: Ok,
                onFailure: Problem
            );
        }
    }
}
