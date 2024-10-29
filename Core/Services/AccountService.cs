using Core.Data.Dto.Account;
using Core.Exceptions;
using Core.IServices;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<UserResponseDto> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var identityUser = registerUserDto.ToIdentityUser();
            await AddUserAsync(identityUser, registerUserDto.Password);

            var userResponse = new UserResponseDto()
            {
                Id = Guid.Parse(identityUser.Id),
                Username = registerUserDto.Username,
                Email = registerUserDto.Email,
                Token = _tokenService.CreateToken(identityUser)
            };

            return userResponse;
        }

        public async Task<UserResponseDto> LoginUserAsync(LoginUserDto loginUserDto)
        {
            IdentityUser? user;
            string name = loginUserDto.Name;

            // Try to find user based on name input
            if (name.Contains('@'))
            {
                user = await _userManager.FindByEmailAsync(name);
            } 
            else
            {
                user = await _userManager.FindByNameAsync(name);
            }
           
            if (user == null)
            {
                throw new UnauthorizedException("Email/Username or Password is invalid.");
            }

            // Try to log sign user in with login information
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginUserDto.Password, false);
            if (!result.Succeeded)
            {
                throw new UnauthorizedException("Email/Username or Password is invalid.");
            }


        }

        private async Task AddUserAsync(IdentityUser identityUser, string password)
        {
            // ADD USER

            var userCreated = await _userManager.CreateAsync(identityUser, password);

            if (!userCreated.Succeeded)
            {
                string err = string.Join(" --- ", userCreated.Errors.Select(err => err.Description));
                throw new RegisterFailedException(err);
            }


            // ADD ROLE TO USER

            var user = await _userManager.FindByEmailAsync(identityUser.Email!);
            var roleAssigned = await _userManager.AddToRoleAsync(user!, "user");

            if (!roleAssigned.Succeeded)
            {
                string err = string.Join(" --- ", roleAssigned.Errors.Select(err => err.Description));
                throw new AddToRoleFailedException(err);
            }
        }
    }
}
