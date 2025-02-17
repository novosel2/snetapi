using Core.Data.Entities;
using Core.Exceptions;
using Core.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            return await _db.Profiles
                .Include(p => p.Followers)
                .Include(p => p.Following)
                .ToListAsync();
        }

        // Return profiles based on user ids
        public async Task<List<Profile>> GetProfilesBatchAsync(List<Guid> userIds)
        {
            return await _db.Profiles.Where(p => userIds.Contains(p.Id)).ToListAsync();
        }

        // Search for profiles based on search term
        public async Task<List<Profile>> SearchProfilesAsync(string searchTerm, Guid currentUserId, int limit = 6)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Profile>();

            var terms = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var query = _db.Profiles.AsQueryable();

            foreach (var term in terms)
            {
                query = query.Where(u =>
                    EF.Functions.ILike(u.FirstName ?? "", $"%{term}%") ||
                    EF.Functions.ILike(u.LastName ?? "", $"%{term}%") ||
                    EF.Functions.ILike(u.Username ?? "", $"%{term}%"));
            }

            return await query
                .Where(p => p.Id != currentUserId)
                .Take(limit)
                .ToListAsync();
        }

        //Gets requested number of most popular profiles
        public async Task<List<Profile>> GetPopularAsync(int limit)
        {
            return await _db.Profiles
                .Select(user => new Profile()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PictureUrl = user.PictureUrl,
                    Followers = user.Followers,
                    Following = user.Following,
                    Username = user.Username,
                    PreviousFollowers = user.PreviousFollowers,
                    // PopularityScore = FollowerWeight * (CurrentFollowers / TheoreticalMaxFollowers) + GrowthWeight * ( (CurrentFollowers - PreviousFollowers) / PreviousFollowers)
                    PopularityScore = 0.8 * ((double)user.Followers.Count / 10000) + 
                                      0.3 * (user.PreviousFollowers > 0 ? 
                                            (user.Followers.Count - user.PreviousFollowers) / user.PreviousFollowers : 0)
                })
                .OrderByDescending(p => p.PopularityScore)
                .Take(limit)
                .ToListAsync();
        }

        // Return profile from database based on ID
        public async Task<Profile?> GetProfileByIdAsync(Guid id)
        {
            return await _db.Profiles
                .Include(p => p.FriendRequestsAsSender)
                .Include(p => p.FriendRequestsAsReceiver)
                .Include(p => p.FriendsAsSender)
                .Include(p => p.FriendsAsReceiver)
                .Include(p => p.Followers)
                .Include(p => p.Following)
                .AsSingleQuery()
                .FirstOrDefaultAsync(up => up.Id == id);
        }

        // Return profile from database based on username
        public async Task<Profile?> GetProfileByUsernameAsync(string username)
        {
            return await _db.Profiles.SingleOrDefaultAsync(p => p.Username == username);
        }

        // Return profile from database based on ID WITHOUT INCLUDING
        public async Task<Profile?> GetProfileByIdAsync_NoInclude(Guid id)
        {
            return await _db.Profiles
                .FirstOrDefaultAsync(up => up.Id == id);
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

        // Check if existing picture url and new url differ
        public async Task<bool> IsUrlDifferentAsync(Guid userId, string pictureUrl)
        {
            return ! await _db.Profiles.AnyAsync(p => p.Id == userId && p.PictureUrl == pictureUrl);
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
