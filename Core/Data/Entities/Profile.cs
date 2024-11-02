using Core.Data.Dto.Account;
using Core.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities
{
    public class Profile
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public Guid UserId { get; set; }

        public ProfileResponse ToProfileResponse()
        {
            return new ProfileResponse()
            {
                Id = Id,
                Username = Username,
                FirstName = FirstName,
                LastName = LastName
            };
        }
    }
}
