using Core.Data.Dto.PostDto;
using Core.Data.Entities;
using Core.Exceptions;
using Core.IRepositories;
using Core.IServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class PostsService : IPostsService
    {
        private readonly IPostsRepository _postRepository;

        public PostsService(IPostsRepository postRepository, IHttpContextAccessor httpContextAccessor)
        {
            _postRepository = postRepository;
        }

        // Get all posts from database
        public async Task<List<PostResponse>> GetPostsAsync()
        {
            List<Post> posts = await _postRepository.GetPostsAsync();

            var postResponses = posts.Select(p => p.ToPostResponse(includeProfile: true)).ToList();

            return postResponses;
        }

        // Get post by post id
        public async Task<PostResponse> GetPostByIdAsync(Guid postId)
        {
            if (! await _postRepository.PostExistsAsync(postId))
            {
                throw new NotFoundException($"Post not found, ID: {postId}");
            }

            Post post = await _postRepository.GetPostByIdAsync(postId);

            var postResponse = post.ToPostResponse();

            return postResponse;
        }

        // Get all posts by user id
        public async Task<List<PostResponse>> GetPostsByUserIdAsync(Guid userId)
        {
            List<Post> posts = await _postRepository.GetPostsByUserIdAsync(userId);

            var postResponses = posts.Select(p => p.ToPostResponse(includeProfile: false)).ToList();

            return postResponses;
        }

        // Add post to database
        public async Task AddPostAsync(PostAddRequest postAddRequest, Guid currentUserId)
        {
            Post post = postAddRequest.ToPost(currentUserId);

            await _postRepository.AddPostAsync(post);

            if (!await _postRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save added post.");
            }
        }

        // Update post with updated information
        public async Task UpdatePostAsync(Guid existingPostId, PostUpdateRequest postUpdateRequest, Guid currentUserId)
        {
            if (! await _postRepository.PostExistsAsync(existingPostId))
            {
                throw new NotFoundException($"Post not found, ID: {existingPostId}");
            }

            Post existingPost = await _postRepository.GetPostByIdAsync(currentUserId);
            Post updatedPost = postUpdateRequest.ToPost(existingPostId, currentUserId);

            if (existingPost.UserId != currentUserId)
            {
                throw new ForbiddenException("You do not have permission to update this post.");
            }

            _postRepository.UpdatePost(existingPost, updatedPost);

            if (!await _postRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save updated post.");
            }
        }

        // Delete post from database
        public async Task DeletePostAsync(Guid postId, Guid currentUserId)
        {
            if (! await _postRepository.PostExistsAsync(postId))
            {
                throw new NotFoundException($"Post not found, ID: {postId}");
            }

            Post post = await _postRepository.GetPostByIdAsync(postId);

            if (post.UserId != currentUserId)
            {
                throw new ForbiddenException("You do not have permission to delete this post.");
            }

            _postRepository.DeletePost(post);

            if (!await _postRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save post deletion.");
            }
        }
    }
}
