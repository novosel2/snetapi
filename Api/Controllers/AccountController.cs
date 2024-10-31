using Core.Data.Dto.Account;
using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("/api/account/")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _config;

        public AccountController(IAccountService accountService, IConfiguration config)
        {
            _accountService = accountService;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto registerUserDto)
        {
            Console.WriteLine(_config["ConnectionStrings:AuthConnection"]);

            UserResponseDto userResponse = await _accountService.RegisterUserAsync(registerUserDto);

            return Ok(userResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginUserDto loginUserDto)
        {
            UserResponseDto userResponse = await _accountService.LoginUserAsync(loginUserDto);

            return Ok(userResponse);
        }

        [HttpPut("update-profile/{profileId}")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(Guid profileId, UpdateProfileDto updateProfileDto)
        {
            UserResponseDto userResponse = await _accountService.UpdateProfileAsync(profileId, updateProfileDto);

            return Ok(userResponse);
        }
    }
}
