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

        public Comment ToComment(Comment existingComment)
        {
            return new Comment()
            {
                Id = existingComment.Id,
                Content = Content,
                UserId = existingComment.UserId,
                PostId = existingComment.PostId,
                ParentCommentId = existingComment.ParentCommentId,
                CreatedOn = existingComment.CreatedOn
            };
        }
    }
}
