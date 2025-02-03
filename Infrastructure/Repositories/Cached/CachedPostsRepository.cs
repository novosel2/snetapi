using Core.Data.Entities;
using Core.Exceptions;
using Core.IRepositories;
using Core.IServices;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Cached
{
    public class CachedPostsRepository : IPostsRepository
    {
        private readonly IDistributedCache _cache;
        private readonly IPostsRepository _postsRepository;
        private readonly IFollowsRepository _followsRepository;
        private readonly Guid _currentUserId;

        public CachedPostsRepository(IDistributedCache cache, IPostsRepository postsRepository, 
            IFollowsRepository followsRepository, ICurrentUserService currentUserService)
        {
            _cache = cache;
            _postsRepository = postsRepository;
            _followsRepository = followsRepository;
            _currentUserId = currentUserService.UserId
                ?? throw new UnauthorizedException("Unauthorized access");
        }

        public async Task<List<Post>> GetPopularFeedAsync(int loadPage)
        {
            if (loadPage >= 10)
            {
                return await _postsRepository.GetPopularFeedAsync(loadPage);
            }

            string key = $"popular-feed-{loadPage}";

            string? cachedPopularFeed = await _cache.GetStringAsync(key);

            List<Post> posts;

            if (cachedPopularFeed == null)
            {
                posts = await _postsRepository.GetPopularFeedAsync(loadPage);

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(posts), new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });

                return posts;
            }

            posts = JsonSerializer.Deserialize<List<Post>>(cachedPopularFeed) 
                ?? throw new JsonDeserializeFailedException("Failed to deserialize popular feed.");

            return posts;
        }

        public async Task<List<Post>> GetYourFeedAsync(List<Guid> friends, List<Guid> followings, int loadPage, Guid currentUserId)
        {
            if (loadPage != 0)
            {
                await _postsRepository.GetYourFeedAsync(friends, followings, loadPage, currentUserId);
            }

            string key = $"your-feed-{currentUserId}";

            string? cachedYourFeed = await _cache.GetStringAsync(key);

            List<Post> posts;

            if (cachedYourFeed == null)
            {
                posts = await _postsRepository.GetYourFeedAsync(friends, followings, loadPage, currentUserId);

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(posts));

                return posts;
            }

            posts = JsonSerializer.Deserialize<List<Post>>(cachedYourFeed)
                ?? throw new JsonDeserializeFailedException("Failed to deserialize popular feed.");

            return posts;
        }

        public async Task<Post?> GetPostByIdAsync(Guid postId)
        {
            string key = $"post-{postId}";

            string? cachedPost = await _cache.GetStringAsync(key);

            Post? post;

            if (cachedPost == null)
            {
                post = await _postsRepository.GetPostByIdAsync(postId);

                if (post != null)
                {
                    await _cache.SetStringAsync(key, JsonSerializer.Serialize<Post>(post), new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(3)
                    });
                }

                return post;
            }

            post = JsonSerializer.Deserialize<Post>(postId)
                ?? throw new JsonDeserializeFailedException("Failed to deserialize popular feed.");

            return post;
        }

        public async Task<List<Post>> GetPostsByUserIdAsync(Guid userId, int loadPage)
        {
            if (loadPage >= 10)
            {
                return await _postsRepository.GetPostsByUserIdAsync(userId, loadPage);
            }

            string key = $"user-feed-{userId}-{loadPage}";

            string? cachedUserFeed = await _cache.GetStringAsync(key);

            List<Post> posts;

            if (cachedUserFeed == null)
            {
                posts = await _postsRepository.GetPostsByUserIdAsync(userId, loadPage);

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(posts), new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(3)
                });

                return posts;
            }

            posts = JsonSerializer.Deserialize<List<Post>>(cachedUserFeed)
                ?? throw new JsonDeserializeFailedException("Failed to deserialize popular feed.");

            return posts;
        }

        public async Task<List<Post>> GetPostsForScoreUpdateAsync(int batchStart, int batchSize)
        {
            return await _postsRepository.GetPostsForScoreUpdateAsync(batchStart, batchSize);
        }

        public async Task AddPostAsync(Post post)
        {
            await _postsRepository.AddPostAsync(post);

            string popularKey = "popular-feed";
            string userFeedKey = $"user-feed-{_currentUserId}";

            for (int i = 0; i < 10; i++)
            {
                await _cache.RemoveAsync($"{popularKey}-{i}");
                await _cache.RemoveAsync($"{userFeedKey}-{_currentUserId}-{i}");
            }

            string yourFeedKey = "your-feed";

            List<Guid> followingIds = ( await _followsRepository.GetAllFollowingUserIdAsync(_currentUserId) ).Select(f => f.FollowerId).ToList();

            foreach (var followingId in followingIds)
            {
                await _cache.RemoveAsync($"{yourFeedKey}-{followingId}");
            }
        }

        public async void DeletePost(Post post)
        {
            string popularKey = "popular-feed";
            string userFeedKey = $"user-feed-{_currentUserId}";

            for (int i = 0; i < 10; i++)
            {
                await _cache.RemoveAsync($"{popularKey}-{i}");
                await _cache.RemoveAsync($"{userFeedKey}-{_currentUserId}-{i}");
            }

            string yourFeedKey = "your-feed";

            var follows = await _followsRepository.GetAllFollowingUserIdAsync(_currentUserId);
            List<Guid> followingIds = follows.Select(f => f.FollowerId).ToList();

            foreach (var followingId in followingIds)
            {
                await _cache.RemoveAsync($"{yourFeedKey}-{followingId}");
            }

            _postsRepository.DeletePost(post);
        }

        public async void UpdatePost(Post existingPost, Post updatedPost)
        {
            _postsRepository.UpdatePost(existingPost, updatedPost);

            string popularKey = "popular-feed";
            string userFeedKey = $"user-feed-{_currentUserId}";

            for (int i = 0; i < 10; i++)
            {
                await _cache.RemoveAsync($"{popularKey}-{i}");
                await _cache.RemoveAsync($"{userFeedKey}-{_currentUserId}-{i}");
            }

            string yourFeedKey = "your-feed";

            List<Guid> followingIds = (await _followsRepository.GetAllFollowingUserIdAsync(_currentUserId)).Select(f => f.FollowerId).ToList();

            foreach (var followingId in followingIds)
            {
                await _cache.RemoveAsync($"{yourFeedKey}-{followingId}");
            }
        }

        public async Task<bool> IsSavedAsync()
        {
            return await _postsRepository.IsSavedAsync();
        }

        public async Task<bool> PostExistsAsync(Guid postId)
        {
            return await _postsRepository.PostExistsAsync(postId);
        }
    }
}
