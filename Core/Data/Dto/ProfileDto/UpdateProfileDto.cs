using Core.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Data.Dto.ProfileDto
{
    public class UpdateProfileDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        [MaxLength(60)]
        public string? Occupation { get; set; }

        public Profile ToProfile(Guid userId)
        {
            return new Profile()
            {
                Id = userId,
                Username = Username,
                FirstName = FirstName,
                LastName = LastName,
                Description = Description,
                Occupation = Occupation
            };
        }
    }
}
