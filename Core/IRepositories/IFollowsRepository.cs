using Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IFollowsRepository
    {
        /// <summary>
        /// Get follow by user id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Follow if found, otherwise null</returns>
        public Task<Follow?> GetFollowByIdsAsync(Guid userId, Guid currentUserId);

        /// <summary>
        /// Adds a follow to the database
        /// </summary>
        /// <param name="follow">Follow object you want to add</param>
        public Task AddFollow(Follow follow);

        /// <summary>
        /// Deletes a follow from the database
        /// </summary>
        /// <param name="follow">Follow object you want to delete</param>
        public void DeleteFollow(Follow follow);

        /// <summary>
        /// Deletes all follows that contain user id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>How many should be deleted</returns>
        public int DeleteFollowsByUserId(Guid userId);

        /// <summary>
        /// Checks if current user already follows the followed
        /// </summary>
        /// <param name="followedId">Followed user</param>
        /// <returns>True if already follows, false if not</returns>
        public Task<bool> FollowExistsAsync(Guid currentUserId, Guid followedId);

        /// <summary>
        /// Check if any changes are saved to database
        /// </summary>
        /// <returns>True if changes are saved, false if not</returns>
        public Task<bool> IsSavedAsync();
    }
}
