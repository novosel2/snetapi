using Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface ICommentsRepository
    {
        /// <summary>
        /// Gets comments by post id
        /// </summary>
        /// <param name="postId">Post id with comments you want to get</param>
        /// <returns>List of comments</returns>
        public Task<List<Comment>> GetCommentsByPostIdAsync(Guid postId);

        /// <summary>
        /// Gets comment by id
        /// </summary>
        /// <param name="commentId">Comment id you want to get</param>
        /// <returns>Comment object</returns>
        public Task<Comment?> GetCommentByIdAsync(Guid commentId);

        /// <summary>
        /// Adds a comment to database
        /// </summary>
        /// <param name="comment">Comment object you want to add</param>
        public Task AddCommentAsync(Comment comment);

        /// <summary>
        /// Deletes comment from database
        /// </summary>
        /// <param name="comment">Comment object you want to delete</param>
        public void DeleteComment(Comment comment);

        /// <summary>
        /// Updates comment in database with new information
        /// </summary>
        /// <param name="existingComment">Existing object in database</param>
        /// <param name="updatedComment">Comment object with updated information</param>
        public void UpdateComment(Comment existingComment, Comment updatedComment);

        /// <summary>
        /// Checks if comment exists in database
        /// </summary>
        /// <param name="commentId">Comment id you want to check</param>
        /// <returns>True if exists, false if not</returns>
        public Task<bool> CommentExistsAsync(Guid commentId);

        /// <summary>
        /// Checks if any changes are saved to database
        /// </summary>
        /// <returns>True if changes are saved, false if not</returns>
        public Task<bool> IsSavedAsync();
    }
}
