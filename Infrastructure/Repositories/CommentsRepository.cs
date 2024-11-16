using Core.Data.Entities;
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
    public class CommentsRepository : ICommentsRepository
    {
        private readonly AppDbContext _db;

        public CommentsRepository(AppDbContext db)
        {
            _db = db;
        }


        // Gets comment by id
        public async Task<Comment?> GetCommentByIdAsync(Guid commentId)
        {
            return await _db.Comments
                .Include(c => c.Post)
                .FirstOrDefaultAsync(c => c.Id == commentId);
        }

        // Adds comment to database
        public async Task AddCommentAsync(Comment comment)
        {
            await _db.Comments.AddAsync(comment);
        }

        // Updates existing comment with new information
        public void UpdateComment(Comment existingComment, Comment updatedComment)
        {
            _db.Comments.Entry(existingComment).CurrentValues.SetValues(updatedComment);
            _db.Comments.Entry(existingComment).State = EntityState.Modified;
        }

        // Deletes comment from database
        public void DeleteComment(Comment comment)
        {
            _db.Comments.Remove(comment);
        }

        // Checks if comment with id exists
        public async Task<bool> CommentExistsAsyn(Guid commentId)
        {
            return await _db.Comments.AnyAsync(c => c.Id == commentId);
        }

        // Checks if any changes are saved to database
        public async Task<bool> IsSavedAsync()
        {
            int saved = await _db.SaveChangesAsync();

            return saved > 0;
        }
    }
}
