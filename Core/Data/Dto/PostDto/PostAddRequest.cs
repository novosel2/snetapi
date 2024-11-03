using Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Dto.PostDto
{
    public class PostAddRequest
    {
        [Required]
        public string Content { get; set; } = string.Empty;

        public Post ToPost(Guid userId)
        {
            return new Post()
            {
                Content = Content,
                UserId = userId
            };
        }
    }
}
