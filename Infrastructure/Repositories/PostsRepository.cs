using Core.Data.Entities;
using Core.Exceptions;
using Core.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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

        // Get posts from database
        public async Task<List<Post>> GetPostsAsync(int loadPage)
        {
            return await _db.Posts
                .Include(p => p.UserProfile)
                .Include(p => p.Reactions)
                .Include(p => p.FileUrls)
                .OrderByDescending(p => p.CreatedOn)
                .Skip(loadPage * 20)
                .Take(20)
                .ToListAsync();
        }

        // Get post by id
        public async Task<Post?> GetPostByIdAsync(Guid postId)
        {
            var post = await _db.Posts
                .Include(p => p.UserProfile)
                .Include(p => p.Reactions)
                .Include(p => p.FileUrls)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
                return null;

            post.Comments = await _db.Comments
                .Where(c => c.PostId == postId && c.ParentCommentId == null)
                .Include(c => c.UserProfile)
                .Include(c => c.Reactions)
                .Include(c => c.CommentReplies)
                    .ThenInclude(cr => cr.UserProfile)
                .Include(c => c.CommentReplies)
                    .ThenInclude(cr => cr.Reactions)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();

            foreach (var comment in post.Comments)
            {
                comment.CommentReplies = comment.CommentReplies.OrderByDescending(c => c.CreatedOn).ToList();
            }

            return post;
        }

        // Get posts by user id
        public async Task<List<Post>> GetPostsByUserIdAsync(Guid userId, int loadPage)
        {
            return await _db.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.UserProfile)
                .Include(p => p.Reactions)
                .Include(p => p.FileUrls)
                .OrderByDescending(p => p.CreatedOn)
                .Skip(loadPage * 20)
                .Take(20)
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
