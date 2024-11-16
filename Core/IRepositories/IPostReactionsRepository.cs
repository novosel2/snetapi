using Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IPostReactionsRepository
    {
        /// <summary>
        /// Gets post reaction by ids
        /// </summary>
        /// <param name="currentUserId">Id of current user</param>
        /// <param name="postId">Id of post</param>
        /// <returns>Post reaction with specified ids</returns>
        public Task<PostReaction?> GetPostReactionByIdAsync(Guid currentUserId, Guid postId);

        /// <summary>
        /// Adds a post reaction to the database
        /// </summary>
        /// <param name="postReaction">Post reaction you want to add</param>
        public Task AddPostReactionAsync(PostReaction postReaction);

        /// <summary>
        /// Deletes post reaction from database
        /// </summary>
        /// <param name="postReaction">Post reaction you want to delete</param>
        public void DeletePostReaction(PostReaction postReaction);

        /// <summary>
        /// Updates post reaction 
        /// </summary>
        /// <param name="postReaction">Post you want to update</param>
        public void UpdatePostReaction(PostReaction postReaction);

        /// <summary>
        /// Checks if post reaction with ids exist
        /// </summary>
        /// <param name="currentUserId">Id of the current user</param>
        /// <param name="postId">Id of the post</param>
        /// <returns>True if exists, false if not</returns>
        public Task<bool> PostReactionExistsAsync(Guid currentUserId, Guid postId);

        /// <summary>
        /// Checks if any changes are saved to database
        /// </summary>
        /// <returns>True if saved, false if not</returns>
        public Task<bool> IsSavedAsync();
    }
}
