using Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Dto.CommentDto
{
    public class CommentAddRequest
    {
        [Required]
        public string Content { get; set; } = string.Empty;

        public Comment ToComment(Guid currentUserId, Guid postId, Guid? commentId = null)
        {
            return new Comment()
            {
                Content = Content,
                UserId = currentUserId,
                PostId = postId,
                ParentCommentId = commentId
            };
        }
    }
}
