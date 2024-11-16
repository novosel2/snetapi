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
        private readonly IBlobStorageService _blobStorageService;
        private readonly UserManager<AppUser> _userManager;
        private readonly Guid _currentUserId;

        public ProfileService(IProfileRepository profileRepository, IBlobStorageService blobStorageService, UserManager<AppUser> userManager, ICurrentUserService currentUserService)
        {
            _profileRepository = profileRepository;
            _blobStorageService = blobStorageService;
            _userManager = userManager;
            _currentUserId = currentUserService.UserId.GetValueOrDefault();
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
            Profile profile = await _profileRepository.GetProfileByIdAsync(userId) 
                ?? throw new NotFoundException($"Profile not found, ID: {userId}");

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
        public async Task<ProfileResponse> UpdateProfileAsync(UpdateProfileDto updateProfileDto)
        {
            Profile existingProfile = await GetProfileAsync(_currentUserId);
            string oldUsername = existingProfile.Username;

            Profile updatedProfile = updateProfileDto.ToProfile(_currentUserId);

            _profileRepository.UpdateProfile(existingProfile, updatedProfile);

            if (!await _profileRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save updated profile.");
            }

            if (updatedProfile.Username != oldUsername)
            {
                AppUser user = ( await _userManager.FindByIdAsync(_currentUserId.ToString()) )!;

                await _userManager.SetUserNameAsync(user, updatedProfile.Username);
            }

            return updatedProfile.ToProfileResponse();
        }

        // Update profile picture
        public async Task<ProfileResponse> UpdateProfilePictureAsync(IFormFile image)
        {
            string pictureUrl = await _blobStorageService.UpdateProfilePictureByUserId(_currentUserId, image);

            Profile profile = await GetProfileAsync(_currentUserId);
            Profile updatedProfile = new Profile()
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Username = profile.Username,
                PictureUrl = pictureUrl
            };

            if (await _profileRepository.IsUrlDifferentAsync(_currentUserId, pictureUrl))
            {
                _profileRepository.UpdateProfile(profile, updatedProfile);

                if (! await _profileRepository.IsSavedAsync())
                {
                    throw new DbSavingFailedException("Failed to save new Picture Url to database.");
                }
            }

            return updatedProfile.ToProfileResponse();
        }

        // Deletes profile picture from user and blob storage
        public async Task<ProfileResponse> DeleteProfilePictureAsync()
        {
            Profile profile = await GetProfileAsync(_currentUserId);

            string defaultPictureUrl = await _blobStorageService.DeleteProfilePictureByUrl(profile.PictureUrl);

            Profile updatedProfile = new Profile()
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Username = profile.Username,
                PictureUrl = defaultPictureUrl
            };

            if (await _profileRepository.IsUrlDifferentAsync(_currentUserId, defaultPictureUrl))
            {
                _profileRepository.UpdateProfile(profile, updatedProfile);

                if (!await _profileRepository.IsSavedAsync())
                {
                    throw new DbSavingFailedException("Failed to save new Picture Url to database.");
                }
            }

            return updatedProfile.ToProfileResponse();
        }

        // Deletes profile from database
        public async Task DeleteProfileAsync()
        {
            Profile profile = await GetProfileAsync(_currentUserId);

            await _blobStorageService.DeleteProfilePictureByUrl(profile.PictureUrl);
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



        private async Task<Profile> GetProfileAsync(Guid currentUserId)
        {
            Profile profile = await _profileRepository.GetProfileByIdAsync(currentUserId)
                ?? throw new NotFoundException($"Profile not found, ID: {currentUserId}");

            return profile;
        }
    }
}