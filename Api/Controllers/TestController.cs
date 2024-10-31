using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("/api/test/")]
    [Authorize]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _config;

        public TestController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Everythin gud :)");
        }

        [HttpGet("auth")]
        [AllowAnonymous]
        public IActionResult GetNoAuth()
        {
            return Ok("goood boyoyy.");
        }
    }
}