using Core.Data.Dto.ProfileDto;
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
        /// Get all profiles from database
        /// </summary>
        /// <returns>List of profile responses</returns>
        public Task<List<ProfileResponse>> GetProfilesAsync();

        /// <summary>
        /// Get profile by profile id
        /// </summary>
        /// <param name="profileId">Profile id</param>
        /// <returns>User profile with specified profile id</returns>
        public Task<ProfileResponse> GetProfileByIdAsync(Guid profileId);

        /// <summary>
        /// Add profile to database
        /// </summary>
        /// <param name="appUser">User that is owner of this profile</param>
        public Task<Profile> AddProfileAsync(AppUser appUser);

        /// <summary>
        /// Updates profile with new information
        /// </summary>
        /// <param name="updateProfileDto">New information</param>
        /// <returns>User Response with new information</returns>
        public Task<ProfileResponse> UpdateProfileAsync(UpdateProfileDto updateProfileDto, Guid currentUserId);

        /// <summary>
        /// Deletes profile based on it's User ID
        /// </summary>
        public Task DeleteProfileAsync(Guid currentUserId);

        /// <summary>
        /// Starts a transaction
        /// </summary>
        /// <returns>Transaction</returns>
        public Task<IDbContextTransaction> StartTransactionAsync();
    }
}
