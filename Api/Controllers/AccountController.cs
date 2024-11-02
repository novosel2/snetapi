using Core.Data.Dto.Account;
using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("/api/account/")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IProfileService _profileService;
        private readonly IConfiguration _config;

        public AccountController(IAccountService accountService, IProfileService profileService, IConfiguration config)
        {
            _accountService = accountService;
            _profileService = profileService;
            _config = config;
        }


        // POST: /api/account/register

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto registerUserDto)
        {
            Console.WriteLine(_config["ConnectionStrings:AuthConnection"]);

            UserResponse userResponse = await _accountService.RegisterUserAsync(registerUserDto);

            return Ok(userResponse);
        }


        // POST: /api/account/login

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginUserDto loginUserDto)
        {
            UserResponse userResponse = await _accountService.LoginUserAsync(loginUserDto);

            return Ok(userResponse);
        }


        // PUT /api/account/update-profile/4ed6d09a-1f46-4670-8d6f-b1fcd8d92ccc

        [Authorize]
        [HttpPut("update-profile/{profileId}")]
        public async Task<IActionResult> UpdateProfile(Guid profileId, UpdateProfileDto updateProfileDto)
        {
            Guid currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            ProfileResponse profileResponseDto = await _profileService.UpdateProfileAsync(profileId, updateProfileDto, currentUserId);

            return Ok(profileResponseDto);
        }


        // DELETE /api/account/delete-user/4ed6d09a-1f46-4670-8d6f-b1fcd8d92ccc

        [Authorize]
        [HttpDelete("delete-profile/{userId}")]
        public async Task<IActionResult> DeleteProfile(Guid userId)
        {
            Guid currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _accountService.DeleteUserAsync(userId, currentUserId);

            return Ok("User successfully deleted.");
        }
    }
}
