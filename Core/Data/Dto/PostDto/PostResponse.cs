using Core.Data.Dto.ProfileDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Dto.PostDto
{
    public class PostResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public ProfileResponse? UserProfile { get; set; }
    }
}
