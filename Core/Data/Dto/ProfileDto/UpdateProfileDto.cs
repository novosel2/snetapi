using Core.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Core.Data.Dto.ProfileDto
{
    public class UpdateProfileDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public Profile ToProfile(Guid userId)
        {
            return new Profile()
            {
                Id = userId,
                Username = Username,
                FirstName = FirstName,
                LastName = LastName,
            };
        }
    }
}
