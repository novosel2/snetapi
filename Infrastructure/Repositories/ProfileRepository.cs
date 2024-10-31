using Core.Data.Entities;
using Core.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly AppDbContext _db;

        public ProfileRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Profile> GetProfileByIdAsync(Guid id)
        {
            return await _db.Profiles.FirstAsync(up => up.Id == id);
        }

        public async Task AddProfileAsync(Profile profile)
        {
            await _db.Profiles.AddAsync(profile);
        }

        public void UpdateProfile(Profile existingProfile, Profile updatedProfile)
        {
            _db.Profiles.Entry(existingProfile).CurrentValues.SetValues(updatedProfile);
            _db.Profiles.Entry(existingProfile).State = EntityState.Modified;
        }

        public async Task<bool> ProfileExistsAsync(Guid id)
        {
            return await _db.Profiles.AnyAsync(up => up.Id == id);
        }

        public async Task<bool> IsSavedAsync()
        {
            int saved = await _db.SaveChangesAsync();

            return saved > 0;
        }
    }
}
