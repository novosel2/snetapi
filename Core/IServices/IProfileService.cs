using Core.Data.Dto.ProfileDto;
using Core.Data.Entities;
using Core.Data.Entities.Identity;
using Microsoft.AspNetCore.Http;
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
        /// Gets requested number of most popular profiles
        /// </summary>
        /// <param name="limit">Number of popular profiles you want to get</param>
        /// <returns>List of most popular profiles</returns>
        public Task<List<ProfileResponse>> GetPopularAsync(int limit);

        /// <summary>
        /// Gets a requested number of follow suggestions, based on current 
        /// users friends followings
        /// </summary>
        /// <param name="limit">Number of suggestions you want to get</param>
        /// <returns>List of suggested profiles</returns>
        public Task<List<SuggestedProfileDto>> GetFollowSuggestionsAsync(int limit);

        /// <summary>
        /// Get profile by profile id
        /// </summary>
        /// <param name="profileId">Profile id</param>
        /// <returns>User profile with specified profile id</returns>
        public Task<ProfileResponse> GetProfileByIdAsync(Guid profileId);

        /// <summary>
        /// Get friendship status between current user and requested user
        /// </summary>
        /// <param name="profileId">User id we want to check status</param>
        /// <returns>Friendship status dto</returns>
        public Task<ProfileFriendshipStatusDto> GetProfileFriendshipStatusAsync(Guid profileId);

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
        public Task<ProfileResponse> UpdateProfileAsync(UpdateProfileDto updateProfileDto);

        /// <summary>
        /// Updates profile picture
        /// </summary>
        /// <param name="image">New profile picture</param>
        public Task<ProfileResponse> UpdateProfilePictureAsync(IFormFile image);

        /// <summary>
        /// Deletes profile picture
        /// </summary>
        /// <returns>Profile response</returns>
        public Task<ProfileResponse> DeleteProfilePictureAsync();

        /// <summary>
        /// Deletes profile based on it's User ID
        /// </summary>
        public Task DeleteProfileAsync();

        /// <summary>
        /// Starts a transaction
        /// </summary>
        /// <returns>Transaction</returns>
        public Task<IDbContextTransaction> StartTransactionAsync();
    }
}
