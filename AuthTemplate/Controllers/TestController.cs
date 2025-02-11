using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthTemplate.Controllers
{
    [ApiController]
    [Route("test")]
    [Authorize]
    public class TestController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Test");
        }

        [HttpGet("testAnonymous")]
        [AllowAnonymous]
        public IActionResult TestAnonymous()
        {
            return Ok("TestAnonymous");
        }
    }
}
