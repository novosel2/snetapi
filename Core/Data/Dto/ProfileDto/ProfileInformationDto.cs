using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Dto.ProfileDto
{
    public class ProfileInformationDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string PictureUrl { get; set; } = string.Empty;
    }
}