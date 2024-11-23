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
    public class FriendshipsRepository : IFriendshipsRepository
    {
        private readonly AppDbContext _db;

        public FriendshipsRepository(AppDbContext db)
        {
            _db = db;
        }

        // Gets all friendships by user id
        public async Task<List<Friendship>> GetFriendshipsByUserIdAsync(Guid userId)
        {
            return await _db.Friendships
                .Where(fs => fs.SenderId == userId || fs.ReceiverId == userId)
                .Include(fs => fs.SenderUser)
                .Include(fs => fs.ReceiverUser)
                .ToListAsync();
        }

        // Get friendship by id
        public async Task<Friendship?> GetFriendshipByIdAsync(Guid friendshipId)
        {
            return await _db.Friendships.FirstOrDefaultAsync(fs => fs.Id == friendshipId);
        }

        // Get friendship by ids
        public async Task<Friendship?> GetFriendshipByIdsAsync(Guid userId, Guid currentUserId)
        {
            return await _db.Friendships
                .FirstOrDefaultAsync(fs => fs.SenderId == userId && fs.ReceiverId == currentUserId 
                || fs.SenderId == currentUserId && fs.ReceiverId == userId);
        }

        // Adds a friendship to database
        public async Task AddFriendshipAsync(Friendship friendship)
        {
            await _db.Friendships.AddAsync(friendship);
        }

        // Deletes a friendship from database
        public void DeleteFriendship(Friendship friendship)
        {
            _db.Friendships.Remove(friendship);
        }

        // Checks if a friendship between two users exists
        public async Task<bool> FriendshipExistsByIdsAsync(Guid userId, Guid currentUserId)
        {
            return await _db.Friendships
                .AnyAsync(fs => fs.SenderId == userId && fs.ReceiverId == currentUserId
                || fs.SenderId == currentUserId && fs.ReceiverId == userId);
        }

        // Checks if any changes are saved to database
        public async Task<bool> IsSavedAsync()
        {
            int saved = await _db.SaveChangesAsync();

            return saved > 0;
        }
    }
}
