using Core.Data.Dto.AccountDto;
using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("/api/account/")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        // POST: /api/account/register

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto registerUserDto)
        {
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


        // DELETE /api/account/delete-user

        [Authorize]
        [HttpDelete("delete-user")]
        public async Task<IActionResult> DeleteUser()
        {
            await _accountService.DeleteUserAsync();

            return Ok();
        }
    }
}