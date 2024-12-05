using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Dto.ProfileDto
{
    public class SuggestedProfileDto
    {
        public ProfileInformationDto User { get; set; } = new();
        public int Mutual { get; set; } = 0;
    }
}
