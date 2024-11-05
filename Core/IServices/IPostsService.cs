using Core.Data.Dto.PostDto;
using Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IPostsService
    {
        /// <summary>
        /// Get all posts from database
        /// </summary>
        /// <returns>List of post responses</returns>
        public Task<List<PostResponse>> GetPostsAsync();

        /// <summary>
        /// Get post by specified id
        /// </summary>
        /// <param name="postId">Id of needed post</param>
        /// <returns>Post response</returns>
        public Task<PostResponse> GetPostByIdAsync(Guid postId);

        /// <summary>
        /// Get all posts made by a specified user
        /// </summary>
        /// <param name="userId">Id of user we want to get posts from</param>
        /// <returns>List of post responses made by specified user</returns>
        public Task<List<PostResponse>> GetPostsByUserIdAsync(Guid userId);

        /// <summary>
        /// Adds post to database
        /// </summary>
        /// <param name="postAddRequest">Post we want to add</param>
        public Task AddPostAsync(PostAddRequest postAddRequest);

        /// <summary>
        /// Updates post with new information
        /// </summary>
        /// <param name="existingPostId">Existing post id we want to update</param>
        public Task UpdatePostAsync(Guid existingPostId, PostUpdateRequest updatedPost);

        /// <summary>
        /// Deletes a post from database
        /// </summary>
        /// <param name="postId">Post id we want to delete</param>
        public Task DeletePostAsync(Guid postId);
    }
}
