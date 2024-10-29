using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Core.Data.Dto.Account
{
    public class RegisterUserDto
    { 
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string ConfirmPassword { get; set; } = string.Empty;

        public IdentityUser ToIdentityUser()
        {
            return new IdentityUser()
            {
                UserName = Username,
                Email = Email
            };
        }
    }
}
