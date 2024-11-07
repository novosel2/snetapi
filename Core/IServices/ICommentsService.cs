using Core.Data.Dto.CommentDto;
using Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface ICommentsService
    {
        /// <summary>
        /// Adds a comment to database
        /// </summary>
        /// <param name="commentAddRequest">Comment object you want to add</param>
        /// <param name="postId">Post id we want to add comment to</param>
        public Task AddCommentAsync(Guid postId, CommentAddRequest commentAddRequest);

        /// <summary>
        /// Deletes comment from database
        /// </summary>
        /// <param name="commentId">Comment id you want to delete</param>
        public Task DeleteCommentAsync(Guid commentId);

        /// <summary>
        /// Updates comment in database with new information
        /// </summary>
        /// <param name="updatedComment">Comment object with updated information</param>
        public Task UpdateCommentAsync(Guid commentId, CommentUpdateRequest updatedComment);
    }
}
