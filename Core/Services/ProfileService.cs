using Core.Data.Dto.Account;
using Core.Data.Entities;
using Core.Data.Entities.Identity;
using Core.Exceptions;
using Core.IRepositories;
using Core.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly UserManager<AppUser> _userManager;

        public ProfileService(IProfileRepository profileRepository, UserManager<AppUser> userManager)
        {
            _profileRepository = profileRepository;
            _userManager = userManager;
        }


        // Get profile by id
        public async Task<Profile> GetProfileByIdAsync(Guid profileId)
        {
            if (! await _profileRepository.ProfileExistsAsync(profileId))
            {
                throw new NotFoundException($"Profile not found, ID: {profileId}");
            }

            Profile profile = await _profileRepository.GetProfileByIdAsync(profileId);

            return profile;
        }

        // Get profile by user id
        public async Task<Profile> GetProfileByUserIdAsync(Guid userId)
        {
            if (! await _profileRepository.ProfileExistsAsync(userId, "user"))
            {
                throw new NotFoundException($"Profile not found, User ID: {userId}");
            }

            Profile profile = await _profileRepository.GetProfileByUserIdAsync(userId);

            return profile;
        }

        // Add profile
        public async Task AddProfileAsync(AppUser appUser)
        {
            Profile profile = new Profile()
            {
                Id = Guid.NewGuid(),
                Username = appUser.UserName,
                UserId = appUser.Id
            };

            await _profileRepository.AddProfileAsync(profile);
        }

        // Update profile with new information
        public async Task<ProfileResponse> UpdateProfileAsync(Guid profileId, UpdateProfileDto updateProfileDto, Guid currentUserId)
        {
            Profile existingProfile = await GetProfileByIdAsync(profileId);
            string oldUsername = existingProfile.Username;

            // Check if current user is the same as the profiles owner
            if (currentUserId != existingProfile.UserId)
            {
                throw new ForbiddenException("You don't have permission to update this user.");
            }

            Profile updatedProfile = updateProfileDto.ToProfile(profileId, existingProfile.UserId);

            await _profileRepository.UpdateProfileAsync(existingProfile, updatedProfile);

            if (updatedProfile.Username != oldUsername)
            {
                AppUser user = ( await _userManager.FindByIdAsync(existingProfile.UserId.ToString()) )!;

                await _userManager.SetUserNameAsync(user, updatedProfile.Username);
            }

            return updatedProfile.ToProfileResponse();
        }

        // Deletes profile from database
        public async Task DeleteProfileAsync(Guid userId)
        {
            Profile profile = await GetProfileByUserIdAsync(userId);

            await _profileRepository.DeleteProfileAsync(profile);
        }

        // Starts a transaction in database
        public async Task<IDbContextTransaction> StartTransactionAsync()
        {
            return await _profileRepository.StartTransactionAsync();
        }
    }
}