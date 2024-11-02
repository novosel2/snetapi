using Core.Data.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.IRepositories
{
    public interface IProfileRepository
    {
        /// <summary>
        /// Gets user profile by id
        /// </summary>
        /// <param name="id">Id we want to find</param>
        /// <returns>User Profile</returns>
        public Task<Profile> GetProfileByIdAsync(Guid id);

        /// <summary>
        /// Get user profile by user id
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>User Profile</returns>
        public Task<Profile> GetProfileByUserIdAsync(Guid userId);

        /// <summary>
        /// Adds a user profile to database
        /// </summary>
        /// <param name="profile">Profile we want to add</param>
        public Task AddProfileAsync(Profile profile);

        /// <summary>
        /// Updates profile with new information
        /// </summary>
        /// <param name="existingProfile">Existing profile in database</param>
        /// <param name="updatedProfile">Profile with new information</param>
        public Task UpdateProfileAsync(Profile existingProfile, Profile updatedProfile);

        /// <summary>
        /// Deletes profile from database based on it's User ID
        /// </summary>
        /// <param name="userId">ID of user we want to delete profile from</param>
        public Task DeleteProfileAsync(Profile profile);

        /// <summary>
        /// Checks if UserProfile exists
        /// </summary>
        /// <param name="id">Id we want to check</param>
        /// <returns>True if exists, false if not</returns>
        public Task<bool> ProfileExistsAsync(Guid id, string type = "profile");

        /// <summary>
        /// Checks if any changes are saved to database
        /// </summary>
        /// <returns>Returns true if any changes are saved, false if not</returns>
        public Task<bool> IsSavedAsync();

        /// <summary>
        /// Starts the transaction
        /// </summary>
        public Task<IDbContextTransaction> StartTransactionAsync();
    }
}
