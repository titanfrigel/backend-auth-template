using BackendAuthTemplate.API.Common.Dtos;
using BackendAuthTemplate.Application.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace BackendAuthTemplate.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected ActionResult Problem(Error error)
        {
            int statusCode = error.ErrorType switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.AccessUnAuthorized => StatusCodes.Status401Unauthorized,
                ErrorType.AccessForbidden => StatusCodes.Status403Forbidden,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };

            ErrorResponse response = new()
            {
                Code = error.Code,
                Message = error.Message,
                Details = error.Details
            };

            return new ObjectResult(response) { StatusCode = statusCode };
        }
    }
}
