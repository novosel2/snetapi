using Core.Data.Dto.ProfileDto;
using Core.Data.Entities;
using Core.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Core.Data.Dto.AccountDto
{
    public class UserResponse
    {
        public ProfileInformationDto User { get; set; } = new();
        public string? Token { get; set; }

        public static UserResponse CreateUserResponse(Profile profile, string token)
        {
            return new UserResponse
            {
                User = profile.ToProfileInformation(),
                Token = token
            };
        }
    }
}
