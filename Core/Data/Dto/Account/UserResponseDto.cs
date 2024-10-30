using Core.Data.Entities;
using Core.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Core.Data.Dto.Account
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public ProfileResponseDto Profile { get; set; } = new ProfileResponseDto();

        public static UserResponseDto CreateUserResonse(AppUser appUser, Profile profile, string token)
        {
            return new UserResponseDto
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
