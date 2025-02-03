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
        private readonly IProfileRepository _profileRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly Guid _currentUserId;

        public PostsService(IPostsRepository postRepository, IProfileRepository profileRepository, 
            IBlobStorageService blobStorageService, ICurrentUserService currentUserService)
        {
            _postRepository = postRepository;
            _profileRepository = profileRepository;
            _blobStorageService = blobStorageService;
            _currentUserId = currentUserService.UserId ?? throw new UnauthorizedException("Unauthorized access.");
        }


        // Get popular feed, most popular posts in last 3 days, then sort by date
        public async Task<List<PostResponse>> GetPopularFeedAsync(int loadPage)
        {
            List<Post> posts = await _postRepository.GetPopularFeedAsync(loadPage);

            var postResponses = posts.Select(p => p.ToPostResponse(_currentUserId)).ToList();

            return postResponses;
        }

        // Get your feed, posts made by your friends or those you follow
        public async Task<List<PostResponse>> GetYourFeedAsync(int loadPage)
        {
            Profile currentUser = await _profileRepository.GetProfileByIdAsync(_currentUserId)
                ?? throw new NotFoundException($"User not found, User ID: {_currentUserId}");

            List<Guid> friendsIds =
            [
                .. currentUser.FriendsAsReceiver.Select(f => f.SenderId).ToList(),
                .. currentUser.FriendsAsSender.Select(f => f.ReceiverId).ToList(),
            ];

            List<Guid> followingIds = currentUser.Following.Select(f => f.FollowedId).ToList();

            List<Post> posts = await _postRepository.GetYourFeedAsync(friendsIds, followingIds, loadPage, _currentUserId);
            List<PostResponse> postResponses = posts.Select(p => p.ToPostResponse(_currentUserId)).ToList();

            return postResponses;
        }

        // Get post by post id
        public async Task<PostResponse> GetPostByIdAsync(Guid postId)
        {
            Post post = await _postRepository.GetPostByIdAsync(postId)
                ?? throw new NotFoundException($"Post not found, ID: {postId}");

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
        public async Task<PostResponse> AddPostAsync(PostAddRequest postAddRequest)
        {
            if (string.IsNullOrEmpty(postAddRequest.Content) && postAddRequest.Files.Count == 0)
            {
                throw new BadRequestException("Content or picture is required.");
            }

            Post post = postAddRequest.ToPost(_currentUserId);

            var uploadTasks = postAddRequest.Files.Select(async file =>
            {
                try
                {
                    string url = await _blobStorageService.UploadPostFile(file);
                    return new FileUrl
                    {
                        PostId = post.Id,
                        Url = url
                    };
                }
                catch (Exception ex)
                {
                    // Log the error and decide whether to rethrow or handle it gracefully
                    Console.WriteLine($"Failed to upload file: {ex.Message}");
                    return null;
                }
            });

            // Filter out null FileUrls caused by failed uploads
            post.FileUrls = (await Task.WhenAll(uploadTasks)).Where(fileUrl => fileUrl != null).ToList()!;

            await _postRepository.AddPostAsync(post);

            if (!await _postRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save added post.");
            }

            post.User = await _profileRepository.GetProfileByIdAsync(_currentUserId);

            return post.ToPostResponse(_currentUserId);
        }

        // Update existing post with updated information
        public async Task UpdatePostAsync(Guid existingPostId, PostUpdateRequest postUpdateRequest)
        {
            Post existingPost = await _postRepository.GetPostByIdAsync(existingPostId)
                ?? throw new NotFoundException($"Post not found, ID: {existingPostId}");
            
            if (existingPost.UserId != _currentUserId)
            {
                throw new ForbiddenException("You do not have permission to update this post.");
            }

            Post updatedPost = postUpdateRequest.ToPost(existingPostId, _currentUserId);
            updatedPost.CommentCount = existingPost.CommentCount;

            var deleteTasks = existingPost.FileUrls.Select(fileUrl => _blobStorageService.DeletePostFile(fileUrl.Url));

            await Task.WhenAll(deleteTasks);

            var uploadTasks = postUpdateRequest.Files.Select(async file =>
            {
                string url = await _blobStorageService.UploadPostFile(file);
                return new FileUrl
                {
                    PostId = existingPostId,
                    Url = url
                };
            });

            updatedPost.FileUrls = (await Task.WhenAll(uploadTasks)).ToList();

            _postRepository.UpdatePost(existingPost, updatedPost);

            if (!await _postRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save updated post.");
            }
        }

        // Delete post from database
        public async Task DeletePostAsync(Guid postId)
        {
            Post post = await _postRepository.GetPostByIdAsync(postId)
                ?? throw new NotFoundException($"Post not found, ID: {postId}");

            if (post.UserId != _currentUserId)
            {
                throw new ForbiddenException("You do not have permission to delete this post.");
            }

            _postRepository.DeletePost(post);

            if (!await _postRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save post deletion.");
            }

            var deleteTasks = post.FileUrls.Select(fileUrl => _blobStorageService.DeletePostFile(fileUrl.Url));

            await Task.WhenAll(deleteTasks);
        }
    }
}
