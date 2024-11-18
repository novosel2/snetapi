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
        private readonly Guid _currentUserId;

        public FriendRequestsService(IFriendRequestsRepository friendRequestsRepository, ICurrentUserService currentUserService)
        {
            _friendRequestsRepository = friendRequestsRepository;
            _currentUserId = currentUserService.UserId ?? throw new UnauthorizedException("Unauthorized access.");
        }

        public async Task<List<FriendRequestResponse>> GetSentFriendRequestsAsync()
        {
            List<FriendRequest> friendRequests = await _friendRequestsRepository.GetFriendRequestsByUserIdAsync(_currentUserId);

            List<FriendRequestResponse> friendRequestReponses = friendRequests
                .Where(fr => fr.SenderId == _currentUserId)
                .Select(fr => fr.ToFriendRequestResponse(_currentUserId))
                .ToList();

            return friendRequestReponses;
        }

        public async Task<List<FriendRequestResponse>> GetReceivedFriendRequestsAsync()
        {
            List<FriendRequest> friendRequests = await _friendRequestsRepository.GetFriendRequestsByUserIdAsync(_currentUserId);

            List<FriendRequestResponse> friendRequestReponses = friendRequests
                .Where(fr => fr.ReceiverId == _currentUserId)
                .Select(fr => fr.ToFriendRequestResponse(_currentUserId))
                .ToList();

            return friendRequestReponses;
        }

        public async Task AddFriendRequestAsync(Guid recieverUserId)
        {
            FriendRequest friendRequest = new FriendRequest()
            {
                SenderId = _currentUserId,
                ReceiverId = recieverUserId
            };

            await _friendRequestsRepository.AddFriendRequestAsync(friendRequest);

            if (!await _friendRequestsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save added friend request to database.");
            }
        }
    }
}
