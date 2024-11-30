using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Dto.ProfileDto
{
    public class SuggestedProfileDto
    {
        public ProfileResponse Profile { get; set; } = new ProfileResponse();
        public int Mutual { get; set; } = 0;
    }
}
