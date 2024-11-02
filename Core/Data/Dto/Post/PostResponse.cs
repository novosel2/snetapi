using Core.Data.Dto.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Dto.Post
{
    public class PostResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public ProfileResponse UserProfile { get; set; } = new ProfileResponse();
    }
}
