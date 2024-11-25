using Core.Data.Dto.FriendsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IFriendshipsService
    {
        /// <summary>
        /// Gets all friendships for specified user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>List of friendship responses for the user</returns>
        public Task<List<FriendshipResponse>> GetFriendshipsByUserIdAsync(Guid userId);

        /// <summary>
        /// Adds a friendship to database
        /// </summary>
        /// <param name="friendRequestId">Id of friend request to be added</param>
        public Task AddFriendshipAsync(Guid friendRequestId);

        /// <summary>
        /// Deletes a friendship from database
        /// </summary>
        /// <param name="friendshipId">Friendship id you want to delete</param>
        public Task DeleteFriendship(Guid friendshipId);

        /// <summary>
        /// Deletes all friendships that contain this user id
        /// </summary>
        public Task DeleteFriendshipsByUserAsync();
    }
}
