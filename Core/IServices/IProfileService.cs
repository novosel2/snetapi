using Core.Data.Dto.Account;
using Core.Data.Entities;
using Core.Data.Entities.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IProfileService
    {
        /// <summary>
        /// Get profile by profile id
        /// </summary>
        /// <param name="profileId">Profile id</param>
        /// <returns>User profile with specified profile id</returns>
        public Task<Profile> GetProfileByIdAsync(Guid profileId);

        /// <summary>
        /// Get profile by user id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User profile with specified user id</returns>
        public Task<Profile> GetProfileByUserIdAsync(Guid userId);

        /// <summary>
        /// Add profile to database
        /// </summary>
        /// <param name="appUser">User that is owner of this profile</param>
        public Task AddProfileAsync(AppUser appUser);

        /// <summary>
        /// Updates profile with new information
        /// </summary>
        /// <param name="updateProfileDto">New information</param>
        /// <returns>User Response with new information</returns>
        public Task<ProfileResponse> UpdateProfileAsync(Guid profileId, UpdateProfileDto updateProfileDto, Guid currentUserId);

        /// <summary>
        /// Deletes profile based on it's User ID
        /// </summary>
        /// <param name="userId">ID of user we want to delete profile from</param>
        public Task DeleteProfileAsync(Guid userId);

        /// <summary>
        /// Starts a transaction
        /// </summary>
        /// <returns>Transaction</returns>
        public Task<IDbContextTransaction> StartTransactionAsync();
    }
}
