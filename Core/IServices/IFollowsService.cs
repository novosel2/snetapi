using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IFollowsService
    {
        /// <summary>
        /// Get all user ids that specified user follows
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of user ids</returns>
        public Task<List<Guid>> GetFollowedByUserIdAsync(Guid userId);

        /// <summary>
        /// Adds a follow to database
        /// </summary>
        /// <param name="followedId">Id of user that is followed by the current user</param>
        public Task AddFollowAsync(Guid followedId);

        /// <summary>
        /// Deletes a follow from database
        /// </summary>
        /// <param name="userId">User id you want to remove follow from</param>
        public Task DeleteFollowAsync(Guid userId);

        /// <summary>
        /// Deletes all follows that contain user id
        /// </summary>
        public Task DeleteFollowsByUserId();
    }
}
