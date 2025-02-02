using Core.Data.Entities;
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
    public class FollowsService : IFollowsService
    {
        private readonly IFollowsRepository _followsRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly Guid _currentUserId;

        public FollowsService(IFollowsRepository followsRepository, IProfileRepository profileRepository, ICurrentUserService currentUserService)
        {
            _followsRepository = followsRepository;
            _profileRepository = profileRepository;
            _currentUserId = currentUserService.UserId.GetValueOrDefault();
        }

        // Get all user ids that specified user follows
        public async Task<List<Guid>> GetFollowedByUserIdAsync(Guid userId)
        {
            List<Follow> follows = await _followsRepository.GetAllFollowsByUserIdAsync(userId);

            var followedIds = follows.Select(f => f.FollowedId).ToList();

            return followedIds;
        }

        // Adds a follow to the database
        public async Task AddFollowAsync(Guid followedId)
        {
            if (await _followsRepository.FollowExistsAsync(_currentUserId, followedId))
            {
                throw new AlreadyExistsException($"Follow already exists, Current User ID: {_currentUserId} | {followedId}");
            }

            Profile currentUser = await _profileRepository.GetProfileByIdAsync(_currentUserId)
                ?? throw new UnauthorizedException("Unauthorized access.");

            Profile followedUser = await _profileRepository.GetProfileByIdAsync(followedId)
                ?? throw new NotFoundException($"User not found, User ID: {followedId}");

            Follow follow = new Follow()
            {
                FollowerId = _currentUserId,
                FollowedId = followedId
            };

            await _followsRepository.AddFollow(follow);

            currentUser.FollowingCount++;
            followedUser.FollowersCount++;

            if (!await _followsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save added follow to database.");
            }
        }

        // Deletes a follow from the database
        public async Task DeleteFollowAsync(Guid userId)
        {
            Follow follow = await _followsRepository.GetFollowByIdsAsync(userId, _currentUserId)
                ?? throw new NotFoundException($"Follow not found, User ID: {userId} | Current User ID: {_currentUserId}");

            Profile currentUser = await _profileRepository.GetProfileByIdAsync(_currentUserId)
                ?? throw new UnauthorizedException("Unauthorized access.");

            Profile unfollowedUser = await _profileRepository.GetProfileByIdAsync(userId)
                ?? throw new NotFoundException($"User not found, User ID: {userId}");

            _followsRepository.DeleteFollow(follow);

            currentUser.FollowingCount--;
            unfollowedUser.FollowersCount--;

            if (!await _followsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save follow deletion to database.");
            }
        }

        // Deletes all follows that contain user id
        public async Task DeleteFollowsByUserId()
        {
            int deleted = _followsRepository.DeleteFollowsByUserId(_currentUserId);

            if (deleted > 0)
            {
                if (!await _followsRepository.IsSavedAsync())
                {
                    throw new DbSavingFailedException("Failed to save follow deletions to database.");
                }
            }
        }
    }
}
