using Core.Data.Entities;

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
        /// Adds a user profile to database
        /// </summary>
        /// <param name="profile">Profile we want to add</param>
        public Task AddProfileAsync(Profile profile);

        /// <summary>
        /// Updates profile with new information
        /// </summary>
        /// <param name="existingProfile">Existing profile in database</param>
        /// <param name="updatedProfile">Profile with new information</param>
        public void UpdateProfile(Profile existingProfile, Profile updatedProfile);

        /// <summary>
        /// Checks if UserProfile exists
        /// </summary>
        /// <param name="id">Id we want to check</param>
        /// <returns>True if exists, false if not</returns>
        public Task<bool> ProfileExistsAsync(Guid id);

        /// <summary>
        /// Checks if any changes are saved to database
        /// </summary>
        /// <returns>Returns true if any changes are saved, false if not</returns>
        public Task<bool> IsSavedAsync();
    }
}
