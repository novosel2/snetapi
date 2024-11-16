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
        private readonly IBlobStorageService _blobStorageService;
        private readonly Guid _currentUserId;

        public PostsService(IPostsRepository postRepository, IBlobStorageService blobStorageService, ICurrentUserService currentUserService)
        {
            _postRepository = postRepository;
            _blobStorageService = blobStorageService;
            _currentUserId = currentUserService.UserId ?? throw new UnauthorizedException("Unauthorized access.");
        }

        // Get all posts from database
        public async Task<List<PostResponse>> GetPostsAsync(int loadPage)
        {
            List<Post> posts = await _postRepository.GetPostsAsync(loadPage);

            var postResponses = posts.Select(p => p.ToPostResponse(_currentUserId)).ToList();

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

            var postResponse = post.ToPostResponse(_currentUserId);

            return postResponse;
        }

        // Get all posts by user id
        public async Task<List<PostResponse>> GetPostsByUserIdAsync(Guid userId, int loadPage)
        {
            List<Post> posts = await _postRepository.GetPostsByUserIdAsync(userId, loadPage);

            var postResponses = posts.Select(p => p.ToPostResponse(_currentUserId)).ToList();

            return postResponses;
        }

        // Add post to database
        public async Task AddPostAsync(PostAddRequest postAddRequest)
        {
            Post post = postAddRequest.ToPost(_currentUserId);
            
            List<FileUrl> fileUrls = [];
            foreach (var file in postAddRequest.Files)
            {
                string url = await _blobStorageService.UploadPostFile(file);
                fileUrls.Add(new FileUrl
                {
                    PostId = post.Id,
                    Url = url
                });
            }

            post.FileUrls = fileUrls;

            await _postRepository.AddPostAsync(post);

            if (!await _postRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save added post.");
            }
        }

        // Update post with updated information
        public async Task UpdatePostAsync(Guid existingPostId, PostUpdateRequest postUpdateRequest)
        {
            if (! await _postRepository.PostExistsAsync(existingPostId))
            {
                throw new NotFoundException($"Post not found, ID: {existingPostId}");
            }

            Post existingPost = await _postRepository.GetPostByIdAsync(existingPostId);
            Post updatedPost = postUpdateRequest.ToPost(existingPostId, _currentUserId);

            foreach (var existingFile in existingPost.FileUrls)
            {
                await _blobStorageService.DeletePostFile(existingFile.Url);
            }

            List<FileUrl> fileUrls = [];
            foreach (var file in postUpdateRequest.Files)
            {
                string url = await _blobStorageService.UploadPostFile(file);
                fileUrls.Add(new FileUrl()
                {
                    PostId = existingPostId,
                    Url = url
                });
            }

            existingPost.FileUrls = fileUrls;

            if (existingPost.UserId != _currentUserId)
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
        public async Task DeletePostAsync(Guid postId)
        {
            if (! await _postRepository.PostExistsAsync(postId))
            {
                throw new NotFoundException($"Post not found, ID: {postId}");
            }

            Post post = await _postRepository.GetPostByIdAsync(postId);

            if (post.UserId != _currentUserId)
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
