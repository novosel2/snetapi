using Core.Data.Entities;
using Core.Exceptions;
using Core.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace Infrastructure.Repositories
{
    public class PostsRepository : IPostsRepository
    {
        private readonly AppDbContext _db;

        public PostsRepository(AppDbContext db)
        {
            _db = db;
        }


        // Get popular feed, most popular posts in last 3 days
        public async Task<List<Post>> GetPopularFeedAsync(int loadPage)
        {
            return await _db.Posts
                .OrderByDescending(p => p.PopularityScore)
                .Skip(loadPage * 20)
                .Take(20)
                .Include(p => p.User)
                .Include(p => p.Reactions)
                .Include(p => p.FileUrls)
                .ToListAsync();
        }

        // Get your feed, posts made by your friends or those you follow
        public async Task<List<Post>> GetYourFeedAsync(List<Guid> friends, List<Guid> followings, int loadPage, Guid currentUserId)
        {
            return await _db.Posts
                .Where(p => friends.Contains(p.UserId) || followings.Contains(p.UserId) || p.UserId == currentUserId)
                .OrderByDescending(p => p.CreatedOn)
                .Skip(loadPage * 20)
                .Take(20)
                .Include(p => p.User)
                .Include(p => p.Reactions)
                .Include(p => p.FileUrls)
                .ToListAsync();
        }

        // Get post by id
        public async Task<Post?> GetPostByIdAsync(Guid postId)
        {
            var post = await _db.Posts
                .Include(p => p.User)
                .Include(p => p.Reactions)
                .Include(p => p.FileUrls)
                .FirstOrDefaultAsync(p => p.Id == postId);

            return post;
        }

        // Get posts from database for popularity score update
        public async Task<List<Post>> GetPostsForScoreUpdateAsync(int batchStart = 0, int batchSize = 1000)
        {
            var twoMonthsAgo = DateTime.Now.AddMonths(-2);

            return await _db.Posts
                .Where(p => p.CreatedOn >= twoMonthsAgo)
                .OrderBy(p => p.Id)
                .Skip(batchStart)
                .Take(batchSize)
                .ToListAsync();
        }

        // Get posts by user id
        public async Task<List<Post>> GetPostsByUserIdAsync(Guid userId, int loadPage)
        {
            return await _db.Posts
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedOn)
                .Skip(loadPage * 20)
                .Take(20)
                .Include(p => p.User)
                .Include(p => p.Reactions)
                .Include(p => p.FileUrls)
                .ToListAsync();
        }

        // Add post to database
        public async Task AddPostAsync(Post post)
        {
            await _db.Posts.AddAsync(post);
        }

        // Update post with new information
        public void UpdatePost(Post existingPost, Post updatedPost)
        {
            _db.Posts.Entry(existingPost).CurrentValues.SetValues(updatedPost);
            _db.Posts.Entry(existingPost).State = EntityState.Modified;
        }

        // Delete post from database
        public void DeletePost(Post post)
        {
            _db.Posts.Remove(post);

        }

        // Check if post with id exists
        public async Task<bool> PostExistsAsync(Guid postId)
        {
            return await _db.Posts.AnyAsync(p => p.Id == postId);
        }

        // Check if any changes are saved to database
        public async Task<bool> IsSavedAsync()
        {
            int saved = await _db.SaveChangesAsync();

            return saved > 0;
        }
    }
}
