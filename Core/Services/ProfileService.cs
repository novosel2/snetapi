using Core.Data.Dto.ProfileDto;
using Core.Data.Entities;
using Core.Data.Entities.Identity;
using Core.Exceptions;
using Core.IRepositories;
using Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Claims;

namespace Core.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly UserManager<AppUser> _userManager;

        public ProfileService(IProfileRepository profileRepository, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _profileRepository = profileRepository;
            _userManager = userManager;
        }

        
        // Get all profiles
        public async Task<List<ProfileResponse>> GetProfilesAsync()
        {
            List<Profile> profiles = await _profileRepository.GetProfilesAsync();

            var profileResponses = profiles.Select(p => p.ToProfileResponse()).ToList();

            return profileResponses;
        }

        // Get profile by id
        public async Task<ProfileResponse> GetProfileByIdAsync(Guid userId)
        {
            if (! await _profileRepository.ProfileExistsAsync(userId))
            {
                throw new NotFoundException($"Profile not found, ID: {userId}");
            }

            Profile profile = await _profileRepository.GetProfileByIdAsync(userId);

            return profile.ToProfileResponse();
        }

        // Add profile
        public async Task<Profile> AddProfileAsync(AppUser appUser)
        {
            Profile profile = new Profile()
            {
                Id = appUser.Id,
                Username = appUser.UserName,
            };

            await _profileRepository.AddProfileAsync(profile);

            if (!await _profileRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save added profile.");
            }

            return profile;
        }

        // Update profile with new information
        public async Task<ProfileResponse> UpdateProfileAsync(UpdateProfileDto updateProfileDto, Guid currentUserId)
        {
            Profile existingProfile = await GetProfile(currentUserId);
            string oldUsername = existingProfile.Username;

            Profile updatedProfile = updateProfileDto.ToProfile(currentUserId);

            _profileRepository.UpdateProfile(existingProfile, updatedProfile);

            if (!await _profileRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save updated profile.");
            }

            if (updatedProfile.Username != oldUsername)
            {
                AppUser user = ( await _userManager.FindByIdAsync(currentUserId.ToString()) )!;

                await _userManager.SetUserNameAsync(user, updatedProfile.Username);
            }

            return updatedProfile.ToProfileResponse();
        }

        // Deletes profile from database
        public async Task DeleteProfileAsync(Guid currentUserId)
        {
            Profile profile = await GetProfile(currentUserId);

            _profileRepository.DeleteProfile(profile);

            if (!await _profileRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save profile deletion.");
            }
        }

        // Starts a transaction in database
        public async Task<IDbContextTransaction> StartTransactionAsync()
        {
            return await _profileRepository.StartTransactionAsync();
        }



        private async Task<Profile> GetProfile(Guid currentUserId)
        {
            if (!await _profileRepository.ProfileExistsAsync(currentUserId))
            {
                throw new NotFoundException($"Profile not found, ID: {currentUserId}");
            }

            Profile profile = await _profileRepository.GetProfileByIdAsync(currentUserId);

            return profile;
        }
    }
}