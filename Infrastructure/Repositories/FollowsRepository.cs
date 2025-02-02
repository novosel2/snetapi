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
    public class FollowsRepository : IFollowsRepository
    {
        private readonly AppDbContext _db;

        public FollowsRepository(AppDbContext db)
        {
            _db = db;
        }

        // Get follow by user id
        public async Task<Follow?> GetFollowByIdsAsync(Guid userId, Guid currentUserId)
        {
            return await _db.Follows
                .FirstOrDefaultAsync(f => f.FollowedId == userId && f.FollowerId == currentUserId || f.FollowedId == currentUserId && f.FollowerId == userId);
        }

        // Get all follows by user id
        public async Task<List<Follow>> GetAllFollowsByUserIdAsync(Guid userId)
        {
            return await _db.Follows
                .Where(f => f.FollowerId == userId)
                .ToListAsync();
        }

        // Add follow to database
        public async Task AddFollow(Follow follow)
        {
            await _db.Follows.AddAsync(follow);
        }

        // Delete follow from database
        public void DeleteFollow(Follow follow)
        {
            _db.Follows.Remove(follow);
        }

        // Delete all follows that contain user id
        public int DeleteFollowsByUserId(Guid userId)
        {
            var follows = _db.Follows.Where(f => f.FollowerId == userId || f.FollowedId == userId).ToList();

            _db.Follows.RemoveRange(follows);

            return follows.Count;
        }

        // Checks if current user already follows the followed user
        public async Task<bool> FollowExistsAsync(Guid currentUserId, Guid followedId)
        {
            return await _db.Follows.AnyAsync(f => f.FollowerId == currentUserId && f.FollowedId == followedId);
        }

        // Check if any changes are saved
        public async Task<bool> IsSavedAsync()
        {
            int saved = await _db.SaveChangesAsync();

            return saved > 0;
        }
    }
}