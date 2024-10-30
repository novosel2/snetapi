using Core.Data.Dto.Account;
using Core.Data.Entities;
using Core.Data.Entities.Identity;
using Core.Exceptions;
using Core.IRepositories;
using Core.IServices;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IProfileRepository _profileRepository;

        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, 
            ITokenService tokenService, IProfileRepository profileRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _profileRepository = profileRepository;
        }

        // Register user to the database, create and return User Response with JWT
        public async Task<UserResponseDto> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            AppUser appUser = registerUserDto.ToAppUser();
            await AddUserAsync(appUser, registerUserDto.Password);

            Profile profile = await GetProfileAsync(appUser.Id);
            string token = _tokenService.CreateToken(appUser);

            UserResponseDto userResponse = UserResponseDto.CreateUserResonse(appUser, profile, token);

            return userResponse;
        }

        // Creates a JWT for user that requested it
        public async Task<UserResponseDto> LoginUserAsync(LoginUserDto loginUserDto)
        {
            AppUser? appUser;
            
            // Check if user name input is EMAIL or USERNAME
            if (loginUserDto.Name.Contains('@'))
            {
                appUser = await _userManager.FindByEmailAsync(loginUserDto.Name);
            }
            else
            {
                appUser = await _userManager.FindByNameAsync(loginUserDto.Name);
            }

            // Check if user exists
            if (appUser == null)
            {
                throw new UnauthorizedException("Email/Username or Password is invalid");
            }

            // Check if login information is correct
            var loginResult = await _signInManager.CheckPasswordSignInAsync(appUser, loginUserDto.Password, false);
            if (!loginResult.Succeeded)
            {
                throw new UnauthorizedException("Email/Username or Password is invalid");
            }

            Profile profile = await GetProfileAsync(appUser.Id);
            string token = _tokenService.CreateToken(appUser);

            UserResponseDto userResponse = UserResponseDto.CreateUserResonse(appUser, profile, token);

            return userResponse;
        }

        // ADD USER TO DATABASE
        private async Task AddUserAsync(AppUser appUser, string password)
        {
            // ADD USER
            var userCreated = await _userManager.CreateAsync(appUser, password);

            if (!userCreated.Succeeded)
            {
                string err = string.Join(" | ", userCreated.Errors.Select(err => err.Description));
                throw new RegisterFailedException(err);
            }

            // ADD ROLE TO USER
            var roleAssigned = await _userManager.AddToRoleAsync(appUser, "user");

            if (!roleAssigned.Succeeded)
            {
                string err = string.Join(" | ", userCreated.Errors.Select(err => err.Description));
                throw new AddToRoleFailedException(err);
            }
        }

        // ADD USER PROFILE TO DATABASE
        private async Task<Profile> GetProfileAsync(Guid id)
        {
            Profile profile;

            if (await _profileRepository.ProfileExistsAsync(id))
            {
                profile = await _profileRepository.GetProfileByIdAsync(id);
                return profile;
            }

            profile = new Profile()
            {
                Id = id
            };

            await _profileRepository.AddProfileAsync(profile);

            if (! await _profileRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save profile to database.");
            }

            return profile;
        }
    }
}
