using Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IFriendRequestsRepository
    {
        /// <summary>
        /// Get friend requests by user id
        /// </summary>
        /// <param name="userId">User id of user with friend requests</param>
        /// <returns>List of friend requests</returns>
        public Task<List<FriendRequest>> GetFriendRequestsByUserIdAsync(Guid userId);

        /// <summary>
        /// Adds a friend requests to database
        /// </summary>
        /// <param name="friendRequest">Friend request you want to add</param>
        public Task AddFriendRequestAsync(FriendRequest friendRequest);

        /// <summary>
        /// Removes a friend request from database
        /// </summary>
        /// <param name="friendRequest">Friend request you want to remove</param>
        public void DeleteFriendRequest(FriendRequest friendRequest);

        /// <summary>
        /// Checks if any changes are saved to the database
        /// </summary>
        /// <returns>True if changes are saved, false if not</returns>
        public Task<bool> IsSavedAsync();
    }
}
