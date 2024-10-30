using Core.Data.Entities;
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
    }
}
