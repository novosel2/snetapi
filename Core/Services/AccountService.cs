using Core.Data.Dto.Account;
using Core.Data.Entities;
using Core.Exceptions;
using Core.IRepositories;
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
        private readonly IProfileRepository _profileRepository;

        public AccountService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, 
            ITokenService tokenService, IProfileRepository profileRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _profileRepository = profileRepository;
        }

        public async Task<UserResponseDto> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var user = registerUserDto.ToIdentityUser();

            await AddUserAsync(user, registerUserDto.Password);
            await AddUserProfileAsync(user.Id);

            var userResponse = await GetUserResponse(user.Id, user);

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

            // Try to sign user in with login information
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginUserDto.Password, false);
            if (!result.Succeeded)
            {
                throw new UnauthorizedException("Email/Username or Password is invalid.");
            }

            UserResponseDto userResponse = await GetUserResponse(user.Id, user);

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

        private async Task AddUserProfileAsync(string id)
        {
            Profile userProfile = new Profile()
            {
                Id = Guid.Parse(id)
            };

            await _profileRepository.AddUserProfileAsync(userProfile);

            if (! await _profileRepository.IsSavedAsync())
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id.ToString());
                    await _userManager.DeleteAsync(user!);
                } catch (Exception) { }

                throw new DbSavingFailedException("Failed to create User Profile for User");
            }
        }

        private async Task<UserResponseDto> GetUserResponse(string id, IdentityUser? user)
        {
            if (user == null)
            {
                user = await _userManager.FindByIdAsync(id);
            }
            if (user == null)
            {
                throw new NotFoundException($"User not found, ID: {id}");
            }

            if (! await _profileRepository.UserProfileExistsAsync(Guid.Parse(id)))
            {
                await AddUserProfileAsync(id);
            }

            Profile profile = await _profileRepository.GetUserProfileByIdAsync(Guid.Parse(id));

            var userResponse = new UserResponseDto()
            {
                Id = Guid.Parse(id),
                Username = user.UserName!,
                Email = user.Email!,
                Token = _tokenService.CreateToken(user),
                Profile = profile.ToProfileResponse()
            };

            return userResponse;
        }
    }
}
