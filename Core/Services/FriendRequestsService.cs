using Core.Data.Dto.FriendsDto;
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
    public class FriendRequestsService : IFriendRequestsService
    {
        private readonly IFriendRequestsRepository _friendRequestsRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IFriendshipsRepository _friendshipsRepository;
        private readonly IFriendshipsService _friendshipsService;
        private readonly Guid _currentUserId;

        public FriendRequestsService(IFriendRequestsRepository friendRequestsRepository, IProfileRepository profileRepository, 
            IFriendshipsRepository friendshipsRepository, IFriendshipsService friendshipsService, ICurrentUserService currentUserService)
        {
            _friendRequestsRepository = friendRequestsRepository;
            _profileRepository = profileRepository;
            _friendshipsRepository = friendshipsRepository;
            _friendshipsService = friendshipsService;
            _currentUserId = currentUserService.UserId.GetValueOrDefault();
        }

        // Get all sent friend requests for current user
        public async Task<List<FriendRequestResponse>> GetSentFriendRequestsAsync()
        {
            List<FriendRequest> friendRequests = await _friendRequestsRepository.GetFriendRequestsByUserIdAsync(_currentUserId);

            List<FriendRequestResponse> friendRequestReponses = friendRequests
                .Where(fr => fr.SenderId == _currentUserId)
                .Select(fr => fr.ToFriendRequestResponse(_currentUserId))
                .ToList();

            return friendRequestReponses;
        }

        // Get all received friend request for current user
        public async Task<List<FriendRequestResponse>> GetReceivedFriendRequestsAsync()
        {
            List<FriendRequest> friendRequests = await _friendRequestsRepository.GetFriendRequestsByUserIdAsync(_currentUserId);

            List<FriendRequestResponse> friendRequestReponses = friendRequests
                .Where(fr => fr.ReceiverId == _currentUserId)
                .Select(fr => fr.ToFriendRequestResponse(_currentUserId))
                .ToList();

            return friendRequestReponses;
        }

        // Add sent friend request for current user
        public async Task AddFriendRequestAsync(Guid receiverUserId)
        {
            if (!await _profileRepository.ProfileExistsAsync(receiverUserId))
            {
                throw new NotFoundException($"User not found, User ID: {receiverUserId}");
            }

            if (await _friendRequestsRepository.SentFriendRequestExistsAsync(_currentUserId, receiverUserId))
            {
                throw new AlreadyExistsException($"Sent friend request already exists, Current User ID: {_currentUserId} | Receiver User ID: {receiverUserId}");
            }

            if (await _friendshipsRepository.FriendshipExistsByIdsAsync(receiverUserId, _currentUserId))
            {
                throw new AlreadyExistsException($"Friendship between two users already exists, Current User ID: {_currentUserId} | Receiver User ID: {receiverUserId}");
            }

            if (await _friendRequestsRepository.ReceivedFriendRequestExistsAsync(_currentUserId, receiverUserId))
            {
                FriendRequest existingFriendRequest = await _friendRequestsRepository.GetFriendRequestByIdsAsync(_currentUserId, receiverUserId)
                    ?? throw new NotFoundException($"Friend request not found, Sender ID: {_currentUserId} | Receiver ID: {receiverUserId}");

                await _friendshipsService.AddFriendshipAsync(existingFriendRequest.Id);

                return;
            }

            FriendRequest friendRequest = new FriendRequest()
            {
                SenderId = _currentUserId,
                ReceiverId = receiverUserId
            };

            await _friendRequestsRepository.AddFriendRequestAsync(friendRequest);

            if (!await _friendRequestsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save added friend request to database.");
            }
        }

        // Delete sent friend request by user id
        public async Task DeleteFriendRequestAsync(Guid userId)
        {
            FriendRequest friendRequest = await _friendRequestsRepository.GetFriendRequestByIdsAsync(userId, _currentUserId)
                ?? throw new NotFoundException($"Friend request not found, User ID: {userId} | Current User ID: {_currentUserId}");

            _friendRequestsRepository.DeleteFriendRequest(friendRequest);

            if (!await _friendRequestsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException($"Failed to saved friend request deletion to database, User ID: {userId} | Current User ID: {_currentUserId}");
            }
        }

        // Deletes all friend requests that contain user id
        public async Task DeleteFriendRequestsByUserAsync()
        {
            int deleted = _friendRequestsRepository.DeleteFriendRequestsByUser(_currentUserId);

            if (deleted > 0)
            {
                if (!await _friendRequestsRepository.IsSavedAsync())
                {
                    throw new DbSavingFailedException("Failed to save friend requests deletion to the database.");
                }
            }
        }
    }
}
