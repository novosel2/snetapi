using Core.Data.Entities;
using Core.Enums;
using Core.IServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CommentReactionsRepository : ICommentReactionsRepository
    {
        private readonly AppDbContext _db;

        public CommentReactionsRepository(AppDbContext db)
        {
            _db = db;
        }


        // Gets comment reaction by ids
        public async Task<CommentReaction?> GetCommentReactionByIdAsync(Guid currentUserId, Guid commentId)
        {
            return await _db.CommentReactions.FirstAsync(pr => pr.UserId == currentUserId && pr.CommentId == commentId);
        }

        // Adds comment reaction to database
        public async Task AddCommentReactionAsync(CommentReaction commentReaction)
        {
            await _db.CommentReactions.AddAsync(commentReaction);
        }

        // Updates comment reaction in database
        public void UpdateCommentReaction(CommentReaction commentReaction)
        {
            if (commentReaction.Reaction is ReactionType.Like)
            {
                commentReaction.Reaction = ReactionType.Dislike;
            }
            else if (commentReaction.Reaction is ReactionType.Dislike)
            {
                commentReaction.Reaction = ReactionType.Like;
            }

            _db.CommentReactions.Entry(commentReaction).State = EntityState.Modified;
        }

        // Deletes comment reaction from database
        public void DeleteCommentReaction(CommentReaction commentReaction)
        {
            _db.CommentReactions.Remove(commentReaction);
        }

        // Checks if comment reaction with ids exist
        public async Task<bool> CommentReactionExistsAsync(Guid currentUserId, Guid commentId)
        {
            return await _db.CommentReactions.AnyAsync(pr => pr.UserId == currentUserId && pr.CommentId == commentId);
        }

        // Checks if any changes are saved to database
        public async Task<bool> IsSavedAsync()
        {
            int saved = await _db.SaveChangesAsync();

            return saved > 0;
        }
    }
}
