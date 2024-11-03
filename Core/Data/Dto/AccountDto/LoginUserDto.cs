using System.ComponentModel.DataAnnotations;

namespace Core.Data.Dto.AccountDto
{
    public class LoginUserDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
