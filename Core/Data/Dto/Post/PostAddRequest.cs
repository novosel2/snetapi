using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Dto.Post
{
    public class PostAddRequest
    {
        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public Guid ProfileId { get; set; }
    }
}
