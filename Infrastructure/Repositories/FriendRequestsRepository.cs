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

        // Checks if any changes are saved to database
        public async Task<bool> IsSavedAsync()
        {
            int saved = await _db.SaveChangesAsync();

            return saved > 0;
        }
    }
}
