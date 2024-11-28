﻿using Core.Data.Entities;
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

        // Adds a follow to the database
        public async Task AddFollowAsync(Guid followedId)
        {
            if (await _followsRepository.FollowExistsAsync(_currentUserId, followedId))
            {
                throw new AlreadyExistsException($"Follow already exists, Current User ID: {_currentUserId} | {followedId}");
            }
            if (!await _profileRepository.ProfileExistsAsync(followedId))
            {
                throw new NotFoundException($"User not found, User ID: {followedId}");
            }

            Follow follow = new Follow()
            {
                FollowerId = _currentUserId,
                FollowedId = followedId
            };

            await _followsRepository.AddFollow(follow);

            if (!await _followsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save added follow to database.");
            }
        }

        // Deletes a follow from the database
        public async Task DeleteFollowAsync(Guid userId)
        {
            Follow follow = await _followsRepository.GetFollowByUserIdAsync(userId)
                ?? throw new NotFoundException($"Follow not found, User Id: {userId}");

            _followsRepository.DeleteFollow(follow);

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
