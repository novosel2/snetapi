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
        /// Get single friend request by id
        /// </summary>
        /// <param name="friendRequestId">Friend request id</param>
        /// <returns>Friend request object if found, otherwise null</returns>
        public Task<FriendRequest?> GetFriendRequestByIdAsync(Guid friendRequestId);

        /// <summary>
        /// Get a single friend request by sender and receiver id
        /// </summary>
        /// <param name="senderId">Sender user id</param>
        /// <param name="receiverId">Receiver user id</param>
        /// <returns>Friend request object if found, otherwise null</returns>
        public Task<FriendRequest?> GetFriendRequestByIdsAsync(Guid senderId, Guid receiverId);

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
        /// Checks if a sent friend request currently exists
        /// </summary>
        /// <param name="currentUserId">Current User id</param>
        /// <param name="receiverUserId">Receiver User id</param>
        /// <returns>True if exists, false if not</returns>
        public Task<bool> SentFriendRequestExistsAsync(Guid currentUserId, Guid receiverUserId);

        /// <summary>
        /// Checks if a received friend request currently exists
        /// </summary>
        /// <param name="currentUserId">Current User id</param>
        /// <param name="senderUserId">Sender User id</param>
        /// <returns>True if exists, false if not</returns>
        public Task<bool> ReceivedFriendRequestExistsAsync(Guid currentUserId, Guid senderUserId);

        /// <summary>
        /// Checks if any changes are saved to the database
        /// </summary>
        /// <returns>True if changes are saved, false if not</returns>
        public Task<bool> IsSavedAsync();
    }
}
