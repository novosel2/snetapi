using Core.Data.Entities;
using Core.Exceptions;
using Core.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly AppDbContext _db;

        public ProfileRepository(AppDbContext db)
        {
            _db = db;
        }

        
        // Return profile from database based on ID
        public async Task<Profile> GetProfileByIdAsync(Guid id)
        {
            return await _db.Profiles.FirstAsync(up => up.Id == id);
        }

        // Return profile from database based on User ID
        public async Task<Profile> GetProfileByUserIdAsync(Guid userId)
        {
            return await _db.Profiles.FirstAsync(p => p.UserId == userId);
        }

        // Add profile to the database
        public async Task AddProfileAsync(Profile profile)
        {
            await _db.Profiles.AddAsync(profile);

            if (! await IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save added profile.");
            }
        }

        // Update existing profile with updated information
        public async Task UpdateProfileAsync(Profile existingProfile, Profile updatedProfile)
        {
            _db.Profiles.Entry(existingProfile).CurrentValues.SetValues(updatedProfile);
            _db.Profiles.Entry(existingProfile).State = EntityState.Modified;

            if (! await IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save updated profile.");
            }
        }

        // Delete profile from database
        public async Task DeleteProfileAsync(Profile profile)
        {
            _db.Profiles.Remove(profile);

            if (!await IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save profile deletion.");
            }
        }

        // Check if profile with id exists
        public async Task<bool> ProfileExistsAsync(Guid id, string type = "profile")
        {
            if (type == "profile")
            {
                return await _db.Profiles.AnyAsync(p => p.Id == id);
            }
            else
            {
                return await _db.Profiles.AnyAsync(p => p.UserId == id);
            }
        }

        // Check if any changes are saved
        public async Task<bool> IsSavedAsync()
        {
            int saved = await _db.SaveChangesAsync();

            return saved > 0;
        }

        // Starts a transaction in db
        public async Task<IDbContextTransaction> StartTransactionAsync()
        {
            return await _db.Database.BeginTransactionAsync();
        }
    }
}
