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
    public class FriendRequestsRepository : IFriendRequestsRepository
    {
        private readonly AppDbContext _db;

        public FriendRequestsRepository(AppDbContext db)
        {
            _db = db;
        }

        // Gets all friend requests by user id
        public async Task<List<FriendRequest>> GetFriendRequestsByUserIdAsync(Guid userId)
        {
            return await _db.FriendRequests
                .Where(fr => fr.SenderId == userId || fr.ReceiverId == userId)
                .Include(fs => fs.SenderUser)
                .Include(fs => fs.ReceiverUser)
                .ToListAsync();
        }

        // Get a single friend request by id
        public async Task<FriendRequest?> GetFriendRequestByIdAsync(Guid friendRequestId)
        {
            return await _db.FriendRequests
                .FirstOrDefaultAsync(fr => fr.Id == friendRequestId);
        }

        // Get a single friend request by sender and receiver id
        public async Task<FriendRequest?> GetFriendRequestByIdsAsync(Guid senderId, Guid receiverId) 
        { 
            return await _db.FriendRequests
                .FirstOrDefaultAsync(fr => fr.SenderId == senderId && fr.ReceiverId == receiverId || fr.SenderId == receiverId && fr.ReceiverId == senderId);
        }

        // Adds a friend request to database
        public async Task AddFriendRequestAsync(FriendRequest friendRequest)
        {
            await _db.FriendRequests
                .AddAsync(friendRequest);
        }

        // Deletes a friend request from database
        public void DeleteFriendRequest(FriendRequest friendRequest)
        {
            _db.FriendRequests
                .Remove(friendRequest);
        }

        // Deletes all friend requests with user id
        public int DeleteFriendRequestsByUser(Guid userId)
        {
            var friendRequestsWithUser = _db.FriendRequests.Where(fr => fr.SenderId == userId || fr.ReceiverId == userId).ToList();

            _db.FriendRequests.RemoveRange(friendRequestsWithUser);

            return friendRequestsWithUser.Count;
        }

        // Checks if a sent friend request already exists
        public async Task<bool> SentFriendRequestExistsAsync(Guid currentUserId, Guid receiverUserId)
        {
            return await _db.FriendRequests
                .AnyAsync(fr => fr.SenderId == currentUserId && fr.ReceiverId == receiverUserId);
        }

        // Checks if a received friend request already exists
        public async Task<bool> ReceivedFriendRequestExistsAsync(Guid currentUserId, Guid senderUserId)
        {
            return await _db.FriendRequests
                .AnyAsync(fr => fr.SenderId ==  senderUserId && fr.ReceiverId == currentUserId);
        }

        // Checks if any changes are saved to database
        public async Task<bool> IsSavedAsync()
        {
            int saved = await _db.SaveChangesAsync();

            return saved > 0;
        }
    }
}
