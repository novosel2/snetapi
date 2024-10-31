using Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Dto.Account
{
    public class UpdateProfileDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required] 
        public string LastName { get; set; } = string.Empty;

        public Profile ToProfile(Guid profileId)
        {
            return new Profile()
            {
                Id = profileId,
                FirstName = FirstName,
                LastName = LastName
            };
        }
    }
}
