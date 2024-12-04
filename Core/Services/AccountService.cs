using Core.Data.Dto.AccountDto;
using Core.Data.Entities;
using Core.Data.Entities.Identity;
using Core.Exceptions;
using Core.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IProfileService _profileService;
        private readonly IFriendRequestsService _friendRequestsService;
        private readonly IFriendshipsService _friendshipsService;
        private readonly IFollowsService _followsService;
        private readonly ITokenService _tokenService;
        private readonly Guid _currentUserId;

        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, 
            ITokenService tokenService, IProfileService profileService, ICurrentUserService currentUserService,
            IFriendRequestsService friendRequestsService, IFriendshipsService friendshipsService, IFollowsService followsService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _profileService = profileService;
            _tokenService = tokenService;
            _currentUserId = currentUserService.UserId.GetValueOrDefault();
            _friendRequestsService = friendRequestsService;
            _friendshipsService = friendshipsService;
            _followsService = followsService;

        }


        // Register user to the database, create and return User Response with JWT
        public async Task<UserResponse> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            AppUser appUser = registerUserDto.ToAppUser();
            await AddUserAsync(appUser, registerUserDto.Password);
            Profile profile = await _profileService.AddProfileAsync(appUser);
            
            string token = _tokenService.CreateToken(appUser);

            UserResponse userResponse = UserResponse.CreateUserResponse(appUser, profile, token);

            return userResponse;
        }

        // Creates a JWT for user that requested it
        public async Task<UserResponse> LoginUserAsync(LoginUserDto loginUserDto)
        {
            AppUser? appUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == loginUserDto.Name || u.UserName == loginUserDto.Name);

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

            UserResponse userResponse = UserResponse.CreateUserResponse(appUser, profile, token);

            return userResponse;
        }

        // Deletes a user from the database
        public async Task DeleteUserAsync()
        {
            using var transaction = await _profileService.StartTransactionAsync();

            try
            {
                AppUser? user = await _userManager.FindByIdAsync(_currentUserId.ToString());

                if (user == null)
                {
                    throw new NotFoundException($"User not found, ID: {_currentUserId}");
                }

                await _followsService.DeleteFollowsByUserId();
                await _friendRequestsService.DeleteFriendRequestsByUserAsync();
                await _friendshipsService.DeleteFriendshipsByUserAsync();
                await _profileService.DeleteProfileAsync();
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