using Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface ICommentReactionsRepository
    {
        /// <summary>
        /// Gets comment reaction by ids
        /// </summary>
        /// <param name="currentUserId">Id of current user</param>
        /// <param name="commentId">Id of comment</param>
        /// <returns>Comment reaction with specified ids</returns>
        public Task<CommentReaction> GetCommentReactionByIdAsync(Guid currentUserId, Guid commentId);

        /// <summary>
        /// Adds a comment reaction to the database
        /// </summary>
        /// <param name="commentReaction">Comment reaction you want to add</param>
        public Task AddCommentReactionAsync(CommentReaction commentReaction);

        /// <summary>
        /// Deletes comment reaction from database
        /// </summary>
        /// <param name="commentReaction">Comment reaction you want to delete</param>
        public void DeleteCommentReaction(CommentReaction commentReaction);

        /// <summary>
        /// Updates comment reaction 
        /// </summary>
        /// <param name="commentReaction">Comment you want to update</param>
        public void UpdateCommentReaction(CommentReaction commentReaction);

        /// <summary>
        /// Checks if comment reaction with ids exist
        /// </summary>
        /// <param name="currentUserId">Id of the current user</param>
        /// <param name="commentId">Id of the comment</param>
        /// <returns>True if exists, false if not</returns>
        public Task<bool> CommentReactionExistsAsync(Guid currentUserId, Guid commentId);

        /// <summary>
        /// Checks if any changes are saved to database
        /// </summary>
        /// <returns>True if saved, false if not</returns>
        public Task<bool> IsSavedAsync();
    }
}
