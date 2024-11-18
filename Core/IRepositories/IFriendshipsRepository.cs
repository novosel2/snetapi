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
        /// Checks if any changes are saved to the database
        /// </summary>
        /// <returns>True if changes are saved, false if not</returns>
        public Task<bool> IsSavedAsync();
    }
}
