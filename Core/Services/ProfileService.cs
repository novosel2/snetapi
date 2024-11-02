using Core.Data.Dto.Account;
using Core.Data.Entities;
using Core.Data.Entities.Identity;
using Core.Exceptions;
using Core.IRepositories;
using Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
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
                UserId = appUser.Id,
                User = appUser
            };

            await _profileRepository.AddProfileAsync(profile);

            if (! await _profileRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to create profile.");
            }
        }

        // Update profile with new information
        public async Task<ProfileResponseDto> UpdateProfileAsync(Guid profileId, UpdateProfileDto updateProfileDto)
        {
            if (! await _profileRepository.ProfileExistsAsync(profileId))
            {
                throw new NotFoundException($"Profile not found, ID: {profileId}");
            }

            Profile existingProfile = await _profileRepository.GetProfileByIdAsync(profileId);
            Profile updatedProfile = updateProfileDto.ToProfile(profileId, existingProfile.UserId);

            _profileRepository.UpdateProfile(existingProfile, updatedProfile);

            if (! await _profileRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to update profile.");
            }

            return updatedProfile.ToProfileResponse();
        }
    }
}
