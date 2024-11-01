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

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public AppUser User { get; set; }

        public Profile()
        {
            User = new AppUser()
            {
                Id = UserId
            };
        }

        public ProfileResponseDto ToProfileResponse()
        {
            return new ProfileResponseDto()
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName
            };
        }
    }
}
