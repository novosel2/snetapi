using Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IPostsRepository
    {
        /// <summary>
        /// Get all posts from database
        /// </summary>
        /// <returns>List of posts</returns>
        public Task<List<Post>> GetPostsAsync(int loadPage);

        /// <summary>
        /// Get post by specified id
        /// </summary>
        /// <param name="postId">Id of needed post</param>
        /// <returns>Post</returns>
        public Task<Post> GetPostByIdAsync(Guid postId);

        /// <summary>
        /// Get all posts made by a specified user
        /// </summary>
        /// <param name="userId">Id of user we want to get posts from</param>
        /// <returns>List of posts made by specified user</returns>
        public Task<List<Post>> GetPostsByUserIdAsync(Guid userId, int loadPage);

        /// <summary>
        /// Adds post to database
        /// </summary>
        /// <param name="post">Post we want to add</param>
        public Task AddPostAsync(Post post);

        /// <summary>
        /// Updates post with new information
        /// </summary>
        /// <param name="existingPost">Existing post we want to update</param>
        /// <param name="updatedPost">Post with updated information</param>
        public void UpdatePost(Post existingPost, Post updatedPost);

        /// <summary>
        /// Deletes a post from database
        /// </summary>
        /// <param name="post">Post we want to delete</param>
        public void DeletePost(Post post);

        /// <summary>
        /// Checks if post with specified id exists
        /// </summary>
        /// <param name="postId">Id we want to check</param>
        /// <returns>True if exists, false if not</returns>
        public Task<bool> PostExistsAsync(Guid postId);

        /// <summary>
        /// Check if any changes are saved to database
        /// </summary>
        /// <returns>True if changes are saved, false if not</returns>
        public Task<bool> IsSavedAsync();
    }
}
