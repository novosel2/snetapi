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
        private readonly Guid _currentUserId;

        public FriendRequestsService(IFriendRequestsRepository friendRequestsRepository, IProfileRepository profileRepository, ICurrentUserService currentUserService)
        {
            _friendRequestsRepository = friendRequestsRepository;
            _profileRepository = profileRepository;
            _currentUserId = currentUserService.UserId ?? throw new UnauthorizedException("Unauthorized access.");
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

            if (await _friendRequestsRepository.ReceivedFriendRequestExistsAsync(_currentUserId, receiverUserId))
            {
                await DeleteFriendRequestByIdsAsync(_currentUserId, receiverUserId);

                // FRIENDSHIPSSERVICE ACCEPT FRIEND REQUEST

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

        // Delete sent friend request by id
        public async Task DeleteFriendRequestAsync(Guid friendRequestId)
        {
            FriendRequest friendRequest = await _friendRequestsRepository.GetFriendRequestByIdAsync(friendRequestId)
                ?? throw new NotFoundException($"Friend request not found, Friend Request ID: {friendRequestId}");

            _friendRequestsRepository.DeleteFriendRequest(friendRequest);

            if (!await _friendRequestsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException($"Failed to saved friend request deletion to database, Friend Request ID: {friendRequestId}");
            }
        }


        private async Task DeleteFriendRequestByIdsAsync(Guid senderId, Guid receiverId)
        {
            FriendRequest friendRequest = await _friendRequestsRepository.GetFriendRequestByIdsAsync(senderId, receiverId)
                    ?? throw new NotFoundException($"Friend request not found, Sender ID: {receiverId} | Receiver ID: {senderId}");

            _friendRequestsRepository.DeleteFriendRequest(friendRequest);

            if (!await _friendRequestsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException($"Failed to saved friend request deletion to database, Friend Request ID: {friendRequest.Id}");
            }
        }
    }
}
