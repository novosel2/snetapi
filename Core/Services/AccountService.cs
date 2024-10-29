using Core.Data.Dto.Account;
using Core.Exceptions;
using Core.IServices;
using Microsoft.AspNetCore.Identity;

namespace Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountService(UserManager<IdentityUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
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
