using Core.Data.Dto.ProfileDto;
using Core.Data.Entities;
using Core.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Core.Data.Dto.AccountDto
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Token { get; set; }
        public ProfileResponse Profile { get; set; } = new ProfileResponse();

        public static UserResponse CreateUserResonse(AppUser appUser, Profile profile, string? token = null)
        {
            return new UserResponse
            {
                Id = appUser.Id,
                Username = appUser.UserName,
                Email = appUser.Email,
                Token = token,
                Profile = profile.ToProfileResponse()
            };
        }
    }
}
