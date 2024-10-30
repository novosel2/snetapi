using Core.Data.Dto.Account;
using System.ComponentModel.DataAnnotations;

namespace Core.Data.Entities
{
    public class Profile
    {
        [Key]
        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public ProfileResponseDto ToProfileResponse()
        {
            return new ProfileResponseDto()
            {
                FirstName = FirstName,
                LastName = LastName
            };
        }
    }
}
