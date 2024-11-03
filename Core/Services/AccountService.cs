using Core.Data.Dto.AccountDto;
using Core.Data.Entities;
using Core.Data.Entities.Identity;
using Core.Exceptions;
using Core.IRepositories;
using Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IProfileService _profileService;
        private readonly ITokenService _tokenService;

        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, 
            ITokenService tokenService, IProfileService profileService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _profileService = profileService;
            _tokenService = tokenService;
        }


        // Register user to the database, create and return User Response with JWT
        public async Task<UserResponse> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            AppUser appUser = registerUserDto.ToAppUser();
            await AddUserAsync(appUser, registerUserDto.Password);
            Profile profile = await _profileService.AddProfileAsync(appUser);
            
            string token = _tokenService.CreateToken(appUser);

            UserResponse userResponse = UserResponse.CreateUserResonse(appUser, profile, token);

            return userResponse;
        }

        // Creates a JWT for user that requested it
        public async Task<UserResponse> LoginUserAsync(LoginUserDto loginUserDto)
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

            Profile profile = (await _profileService.GetProfileByIdAsync(appUser.Id)).ToProfile();
            string token = _tokenService.CreateToken(appUser);

            UserResponse userResponse = UserResponse.CreateUserResonse(appUser, profile, token);

            return userResponse;
        }

        // Deletes a user from the database
        public async Task DeleteUserAsync(Guid currentUserId)
        {
            using var transaction = await _profileService.StartTransactionAsync();

            try
            {
                AppUser? user = await _userManager.FindByIdAsync(currentUserId.ToString());

                if (user == null)
                {
                    throw new NotFoundException($"User not found, ID: {currentUserId}");
                }

                await _profileService.DeleteProfileAsync(currentUserId);
                await _userManager.DeleteAsync(user);

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
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
                throw new RoleAssignFailedException(err);
            }
        }

    }
}
