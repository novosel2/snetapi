using Microsoft.AspNetCore.Identity;

namespace Core.Data.Dto.Account
{
    public class UserResponseDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
