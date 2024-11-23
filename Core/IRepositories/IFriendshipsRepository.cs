using Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IFriendshipsRepository
    {
        /// <summary>
        /// Gets all friendships by user id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>List of friendships</returns>
        public Task<List<Friendship>> GetFriendshipsByUserIdAsync(Guid userId);

        /// <summary>
        /// Gets friendship by id
        /// </summary>
        /// <param name="friendshipId">Friendship id</param>
        /// <returns>Friendship if found, otherwise null</returns>
        public Task<Friendship?> GetFriendshipByIdAsync(Guid friendshipId);

        /// <summary>
        /// Gets friendship by ids
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="currentUserId">Current user id</param>
        /// <returns>Friendship if found, otherwise null</returns>
        public Task<Friendship?> GetFriendshipByIdsAsync(Guid userId, Guid currentUserId);

        /// <summary>
        /// Adds friendship to database
        /// </summary>
        /// <param name="friendship">Friendship you want to add</param>
        public Task AddFriendshipAsync(Friendship friendship);

        /// <summary>
        /// Deletes friendship from database
        /// </summary>
        /// <param name="friendship">Friendship you want to delete</param>
        public void DeleteFriendship(Friendship friendship);

        /// <summary>
        /// Checks if friendship already exists between two users
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="currentUserId">Current user id</param>
        /// <returns>True if exists, false if not</returns>
        public Task<bool> FriendshipExistsByIdsAsync(Guid userId, Guid currentUserId);

        /// <summary>
        /// Checks if any changes are saved to the database
        /// </summary>
        /// <returns>True if changes are saved, false if not</returns>
        public Task<bool> IsSavedAsync();
    }
}
