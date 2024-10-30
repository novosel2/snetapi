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

        public async Task<Profile> GetUserProfileByIdAsync(Guid id)
        {
            return await _db.UserProfiles.FirstAsync(up => up.Id == id);
        }

        public async Task AddUserProfileAsync(Profile profile)
        {
            await _db.UserProfiles.AddAsync(profile);
        }

        public async Task<bool> UserProfileExistsAsync(Guid id)
        {
            return await _db.UserProfiles.AnyAsync(up => up.Id == id);
        }

        public async Task<bool> IsSavedAsync()
        {
            int saved = await _db.SaveChangesAsync();

            return saved > 0;
        }
    }
}
