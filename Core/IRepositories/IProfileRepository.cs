﻿using Core.Data.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.IRepositories
{
    public interface IProfileRepository
    {
        /// <summary>
        /// Get all profiles
        /// </summary>
        /// <returns>List of profiles</returns>
        public Task<List<Profile>> GetProfilesAsync();

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
        /// Deletes profile from database based on it's User ID
        /// </summary>
        /// <param name="profile">Profile of user we want to delete</param>
        public void DeleteProfile(Profile profile);

        /// <summary>
        /// Checks if UserProfile exists
        /// </summary>
        /// <param name="id">Id we want to check</param>
        /// <returns>True if exists, false if not</returns>
        public Task<bool> ProfileExistsAsync(Guid id);

        /// <summary>
        /// Starts the transaction
        /// </summary>
        public Task<IDbContextTransaction> StartTransactionAsync();

        /// <summary>
        /// Check if any changes are saved to database
        /// </summary>
        /// <returns>True if changes are saved, false if not</returns>
        public Task<bool> IsSavedAsync();
    }
}
