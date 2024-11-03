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

        
        // Return all profiles from database
        public async Task<List<Profile>> GetProfilesAsync()
        {
            return await _db.Profiles.ToListAsync();
        }

        // Return profile from database based on ID
        public async Task<Profile> GetProfileByIdAsync(Guid id)
        {
            return await _db.Profiles.FirstAsync(up => up.Id == id);
        }

        // Add profile to the database
        public async Task AddProfileAsync(Profile profile)
        {
            await _db.Profiles.AddAsync(profile);
        }

        // Update existing profile with updated information
        public void UpdateProfile(Profile existingProfile, Profile updatedProfile)
        {
            _db.Profiles.Entry(existingProfile).CurrentValues.SetValues(updatedProfile);
            _db.Profiles.Entry(existingProfile).State = EntityState.Modified;
        }

        // Delete profile from database
        public void DeleteProfile(Profile profile)
        {
            _db.Profiles.Remove(profile);
        }

        // Check if profile with id exists
        public async Task<bool> ProfileExistsAsync(Guid id)
        {
            return await _db.Profiles.AnyAsync(p => p.Id == id);
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
