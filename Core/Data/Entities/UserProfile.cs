using System.ComponentModel.DataAnnotations;

namespace Core.Data.Entities
{
    public class UserProfile
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;
    }
}
