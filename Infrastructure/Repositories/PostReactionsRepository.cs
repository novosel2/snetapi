using Core.Data.Entities;
using Core.Enums;
using Core.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PostReactionsRepository : IPostReactionsRepository
    {
        private readonly AppDbContext _db;

        public PostReactionsRepository(AppDbContext db)
        {
            _db = db;
        }


        // Gets post reaction by ids
        public async Task<PostReaction> GetPostReactionByIdAsync(Guid currentUserId, Guid postId)
        {
            return await _db.PostReactions.FirstAsync(pr => pr.UserId == currentUserId && pr.PostId == postId);
        }

        // Adds post reaction to database
        public async Task AddPostReactionAsync(PostReaction postReaction)
        {
            await _db.AddAsync(postReaction);
        }

        // Updates post reaction in database
        public void UpdatePostReaction(PostReaction postReaction)
        {
            if (postReaction.Reaction is ReactionType.Like)
            {
                postReaction.Reaction = ReactionType.Dislike;
            }
            else if (postReaction.Reaction is ReactionType.Dislike)
            {
                postReaction.Reaction = ReactionType.Like;
            }

            _db.PostReactions.Entry(postReaction).State = EntityState.Modified;
        }

        // Deletes post reaction from database
        public void DeletePostReaction(PostReaction postReaction)
        {
            _db.PostReactions.Remove(postReaction);
        }

        // Checks if post reaction with ids exist
        public async Task<bool> PostReactionExistsAsync(Guid currentUserId, Guid postId)
        {
            return await _db.PostReactions.AnyAsync(pr => pr.UserId == currentUserId && pr.PostId == postId);
        }

        // Checks if any changes are saved to database
        public async Task<bool> IsSavedAsync()
        {
            int saved = await _db.SaveChangesAsync();

            return saved > 0;
        }
    }
}
