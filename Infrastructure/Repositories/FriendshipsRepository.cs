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

        // Checks if any changes are saved to database
        public async Task<bool> IsSavedAsync()
        {
            int saved = await _db.SaveChangesAsync();

            return saved > 0;
        }
    }
}
