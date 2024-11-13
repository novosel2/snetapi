﻿using Core.Data.Entities;
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
                .Include(p => p.Comments.OrderByDescending(c => c.CreatedOn).Take(3)).ThenInclude(c => c.UserProfile)
                .Include(p => p.Comments.OrderByDescending(c => c.CreatedOn).Take(3)).ThenInclude(c => c.Reactions)
                .OrderByDescending(p => p.CreatedOn)
                .Skip(loadPage * 20)
                .Take(20)
                .ToListAsync();
        }

        // Get post by id
        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _db.Posts
                .Include(p => p.UserProfile)
                .Include(p => p.Reactions)
                .Include(p => p.Comments.OrderByDescending(c => c.CreatedOn)).ThenInclude(c => c.UserProfile)
                .Include(p => p.Comments.OrderByDescending(c => c.CreatedOn)).ThenInclude(c => c.Reactions)
                .OrderByDescending(p => p.CreatedOn)
                .FirstAsync(p => p.Id == postId);
        }

        // Get posts by user id
        public async Task<List<Post>> GetPostsByUserIdAsync(Guid userId, int loadPage)
        {
            return await _db.Posts
                .Include(p => p.UserProfile)
                .Include(p => p.Reactions)
                .Include(p => p.Comments.OrderByDescending(c => c.CreatedOn).Take(3)).ThenInclude(c => c.UserProfile)
                .Include(p => p.Comments.OrderByDescending(c => c.CreatedOn).Take(3)).ThenInclude(c => c.Reactions)
                .Where(p => p.UserId == userId)
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
