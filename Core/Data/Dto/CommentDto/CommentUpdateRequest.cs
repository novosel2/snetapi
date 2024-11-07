using Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Dto.CommentDto
{
    public class CommentUpdateRequest
    {
        [Required]
        public string Content { get; set; } = string.Empty;

        public Comment ToComment(Guid commentId, Guid currentUserId, Guid postId)
        {
            return new Comment()
            {
                Id = commentId,
                Content = Content,
                UserId = currentUserId,
                PostId = postId
            };
        }
    }
}
