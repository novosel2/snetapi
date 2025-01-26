using Core.Data.Entities;
using Microsoft.AspNetCore.Http;
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
        public string? Content { get; set; } = string.Empty;

        public List<IFormFile> Files { get; set; } = [];

        public Post ToPost(Guid currentUserId)
        {
            return new Post()
            {
                Content = Content,
                UserId = currentUserId
            };
        }
    }
}
