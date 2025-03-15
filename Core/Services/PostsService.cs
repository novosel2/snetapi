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
        private readonly string _role;

        public PostsService(IPostsRepository postRepository, IProfileRepository profileRepository, 
            IBlobStorageService blobStorageService, ICurrentUserService currentUserService)
        {
            _postRepository = postRepository;
            _profileRepository = profileRepository;
            _blobStorageService = blobStorageService;
            _currentUserId = currentUserService.UserId ?? throw new UnauthorizedException("Unauthorized access.");
            _role = currentUserService.Role ?? "user";
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

        // Get all posts by username
        public async Task<List<PostResponse>> GetPostsByUsernameAsync(string username, int loadPage)
        {
            List<Post> posts = await _postRepository.GetPostsByUsernameAsync(username, loadPage);

            List<PostResponse> postResponses = [];
            if (posts.Count > 0)
            {
                postResponses = posts.Select(p => p.ToPostResponse(_currentUserId)).ToList();
            }

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


            var uploadTasks = postAddRequest.Files.Select(file => _blobStorageService.UploadPostFile(file));
            post.FileUrls = (await Task.WhenAll(uploadTasks))
                .Where(url => url != null)
                .Select(url => new FileUrl
                {
                    PostId = post.Id,
                    Url = url
                })
                .ToList();

            await _postRepository.AddPostAsync(post);

            if (!await _postRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save added post.");
            }

            post.User = await _profileRepository.GetProfileByIdAsync(_currentUserId);

            return post.ToPostResponse(_currentUserId);
        }

        // Update existing post with updated information
        public async Task<PostResponse> UpdatePostAsync(Guid existingPostId, PostUpdateRequest postUpdateRequest)
        {
            Post existingPost = await _postRepository.GetPostByIdAsync(existingPostId)
                ?? throw new NotFoundException($"Post not found, ID: {existingPostId}");
            
            if (existingPost.UserId != _currentUserId && _role != "admin")
            {
                throw new ForbiddenException("You do not have permission to update this post.");
            }

            foreach (var file in postUpdateRequest.Files)
            {
                // Ensure it's an image based on MIME type
                if (!file.ContentType.StartsWith("image/"))
                {
                    throw new BadRequestException("Uploaded file is not a valid image.");
                }

                // Ensure it's a video based on MIME type
                else if (!file.ContentType.StartsWith("video/"))
                {
                    throw new BadRequestException("Uploaded file is not a valid video.");
                }
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

            List<FileUrl> fileUrls = (await Task.WhenAll(uploadTasks)).ToList();
            updatedPost.FileUrls = fileUrls;

            var profile = await _profileRepository.GetProfileByIdAsync_NoInclude(_currentUserId);
            updatedPost.User = profile;

            _postRepository.UpdatePost(existingPost, updatedPost);

            if (fileUrls.Count > 0)
                await _postRepository.UpdatePostFileUrls(fileUrls);

            if (!await _postRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save updated post.");
            }

            return updatedPost.ToPostResponse(_currentUserId);
        }

        // Delete post from database
        public async Task DeletePostAsync(Guid postId)
        {
            Post post = await _postRepository.GetPostByIdAsync(postId)
                ?? throw new NotFoundException($"Post not found, ID: {postId}");

            if (post.UserId != _currentUserId && _role != "admin")
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
