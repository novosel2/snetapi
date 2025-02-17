using Core.Data.Dto.ProfileDto;
using Core.Data.Entities;
using Core.Data.Entities.Identity;
using Core.Enums;
using Core.Exceptions;
using Core.IRepositories;
using Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IFriendshipsRepository _friendshipsRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly UserManager<AppUser> _userManager;
        private readonly Guid _currentUserId;

        public ProfileService(IProfileRepository profileRepository, IFriendshipsRepository friendshipsRepository, 
            IBlobStorageService blobStorageService, UserManager<AppUser> userManager, ICurrentUserService currentUserService)
        {
            _profileRepository = profileRepository;
            _friendshipsRepository = friendshipsRepository;
            _blobStorageService = blobStorageService;
            _userManager = userManager;
            _currentUserId = currentUserService.UserId.GetValueOrDefault();
        }


        // Get all profiles
        public async Task<List<ProfileInformationDto>> GetProfilesAsync()
        {
            List<Profile> profiles = await _profileRepository.GetProfilesAsync();

            List<ProfileInformationDto> profileResponses = profiles.Select(p => p.ToProfileInformation()).ToList();

            return profileResponses;
        }

        // Get profiles based on user ids
        public async Task<List<ProfileInformationDto>> GetProfilesBatchAsync(List<Guid> userIds)
        {
            List<Profile> profiles = await _profileRepository.GetProfilesBatchAsync(userIds);

            var profileResponses  = profiles.Select(p => p.ToProfileInformation()).ToList();

            return profileResponses;
        }

        // Search for profiles based on search term
        public async Task<List<ProfileInformationDto>> SearchProfilesAsync(string searchTerm, int limit = 6)
        {
            List<Profile> profiles = await _profileRepository.SearchProfilesAsync(searchTerm, _currentUserId, limit);

            var profileResponses = profiles.Select(p => p.ToProfileInformation()).ToList();

            return profileResponses;
        }

        // Gets requested number of most popular profiles
        public async Task<List<ProfileInformationDto>> GetPopularAsync(int limit)
        {
            List<Profile> profiles = await _profileRepository.GetPopularAsync(limit);

            List<ProfileInformationDto> profileResponses = profiles.Select(p => p.ToProfileInformation()).ToList();

            return profileResponses;
        }

        // Gets a requested number of follow suggestions, based on current users friends followings
        public async Task<List<Guid>> GetFollowSuggestionsAsync(int limit)
        {
            Profile currentUser = await _profileRepository.GetProfileByIdAsync(_currentUserId)
                ?? throw new UnauthorizedException("Unauthorized access.");

            List<Guid> friendsFollowing = await _friendshipsRepository.GetFriendsFollowingsAsync(_currentUserId);

            var currentUserFollowingIds = currentUser.Following.Select(f => f.FollowedId).ToHashSet();

            HashSet<Guid> suggestedUserIds = friendsFollowing
                .Where(followedId => !currentUserFollowingIds.Contains(followedId) && followedId != _currentUserId)
                .ToHashSet();

            var random = new Random();
            return suggestedUserIds
                .OrderBy(x => random.Next())
                .Take(limit)
                .ToList();
        }

        // Gets a requested number of mutual friends between current user and the specified user
        public async Task<List<Guid>> GetMutualFriendsIdsAsync(Guid userId, int limit)
        {
            // get current user
            Profile currentUser = await _profileRepository.GetProfileByIdAsync(_currentUserId)
                ?? throw new UnauthorizedException("Unauthorized access");
            // get user by userid
            Profile user = await _profileRepository.GetProfileByIdAsync(userId)
                ?? throw new NotFoundException($"User not found, User ID: {userId}");
            // check mutual friendships
            List<Guid> currentUserFriends = [.. currentUser.FriendsAsReceiver
                .Select(f => f.SenderId).ToList(), .. currentUser.FriendsAsSender.Select(f => f.ReceiverId).ToList()];

            List<Guid> userFriends = [.. user.FriendsAsReceiver
                .Select(f => f.SenderId).ToList(), .. user.FriendsAsSender.Select(f => f.ReceiverId).ToList()];

            var mutual = currentUserFriends.Intersect(userFriends).ToList();

            Random rnd = new Random();
            mutual = mutual.OrderBy(_ => rnd.Next())
                .Take(limit)
                .ToList();

            return mutual;
        }

        // Get profile by id
        public async Task<ProfileResponse> GetProfileByIdAsync(Guid userId)
        {
            Profile profile = await _profileRepository.GetProfileByIdAsync(userId) 
                ?? throw new NotFoundException($"Profile not found, ID: {userId}");

            return profile.ToProfileResponse();
        }

        // Get profile by username
        public async Task<ProfileResponse> GetProfileByUsernameAsync(string username)
        {
            Profile? profile = await _profileRepository.GetProfileByUsernameAsync(username)
                ?? throw new NotFoundException($"User not found, Username: {username}");

            return profile.ToProfileResponse();
        }

        // Get profile by id WITHOUT INCLUDING
        public async Task<ProfileResponse> GetProfileByIdAsync_NoInclude(Guid userId)
        {
            Profile profile = await _profileRepository.GetProfileByIdAsync_NoInclude(userId)
                ?? throw new NotFoundException($"Profile not found, ID: {userId}");

            return profile.ToProfileResponse();
        }

        // Get friendship status between current user and requested user
        public async Task<ProfileFriendshipStatusDto> GetProfileFriendshipStatusAsync(Guid userId)
        {
            Profile profile = await _profileRepository.GetProfileByIdAsync(userId)
                ?? throw new NotFoundException($"Profile not found, ID: {userId}");

            ProfileFriendshipStatusDto friendshipStatus = new ProfileFriendshipStatusDto()
            {
                UserId = userId
            };

            if (profile.Followers.Any(f => f.FollowerId == _currentUserId))
                friendshipStatus.IsFollowed = true;

            if (profile.FriendsAsSender.Any(f => f.ReceiverId == _currentUserId) || profile.FriendsAsReceiver.Any(f => f.SenderId == _currentUserId))
                friendshipStatus.FriendshipStatus = Status.Friends;

            else if (profile.FriendRequestsAsReceiver.Any(f => f.SenderId == _currentUserId))
                friendshipStatus.FriendshipStatus = Status.SentRequest;

            else if (profile.FriendRequestsAsSender.Any(f => f.ReceiverId == _currentUserId))
                friendshipStatus.FriendshipStatus = Status.ReceivedRequest;

            return friendshipStatus;
        }

        // Add profile
        public async Task AddProfileAsync(AppUser appUser)
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
        }

        // Update profile with new information
        public async Task<ProfileInformationDto> UpdateProfileAsync(UpdateProfileDto updateProfileDto)
        {
            Profile existingProfile = await GetProfileAsync(_currentUserId);
            string oldUsername = existingProfile.Username;

            AppUser? appUser = await _userManager.FindByNameAsync(updateProfileDto.Username);
            if(appUser != null && appUser.UserName != existingProfile.Username)
            {
                throw new AlreadyExistsException("Username taken");
            }

            Profile updatedProfile = updateProfileDto.ToProfile(_currentUserId);
            updatedProfile.PictureUrl = existingProfile.PictureUrl;
            updatedProfile.FollowersCount = existingProfile.FollowersCount;
            updatedProfile.FollowingCount = existingProfile.FollowingCount;
            updatedProfile.PreviousFollowers = existingProfile.PreviousFollowers;

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

            return updatedProfile.ToProfileInformation();
        }

        // Update profile picture
        public async Task<ProfileInformationDto> UpdateProfilePictureAsync(IFormFile image)
        {
            string pictureUrl = await _blobStorageService.UpdateProfilePictureByUserId(_currentUserId, image);

            string pictureUrlTimeStamped = $"{pictureUrl}?{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";

            Profile profile = await GetProfileAsync(_currentUserId);
            Profile updatedProfile = new Profile()
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Username = profile.Username,
                PictureUrl = pictureUrlTimeStamped
            };

            _profileRepository.UpdateProfile(profile, updatedProfile);

            if (! await _profileRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save new Picture Url to database.");
            }

            return updatedProfile.ToProfileInformation();
        }

        // Deletes profile picture from user and blob storage
        public async Task<ProfileInformationDto> DeleteProfilePictureAsync()
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

            return updatedProfile.ToProfileInformation();
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