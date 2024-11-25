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
    public class FriendshipsService : IFriendshipsService
    {
        private readonly IFriendshipsRepository _friendshipsRepository;
        private readonly IFriendRequestsRepository _friendRequestsRepository;
        private readonly Guid _currentUserId;

        public FriendshipsService(IFriendshipsRepository friendsRepository, IFriendRequestsRepository friendRequestsRepository, ICurrentUserService currentUserService)
        {
            _friendshipsRepository = friendsRepository;
            _friendRequestsRepository = friendRequestsRepository;
            _currentUserId = currentUserService.UserId.GetValueOrDefault();
        }

        // Gets all friendships by user id
        public async Task<List<FriendshipResponse>> GetFriendshipsByUserIdAsync(Guid userId)
        {
            List<Friendship> friendships = await _friendshipsRepository.GetFriendshipsByUserIdAsync(userId);

            List<FriendshipResponse> friendshipResponses = friendships.Select(f => f.ToFriendshipResponse(userId)).ToList();

            return friendshipResponses;
        }

        // Adds a friendship to database
        public async Task AddFriendshipAsync(Guid friendRequestId)
        {
            FriendRequest friendRequest = await _friendRequestsRepository.GetFriendRequestByIdAsync(friendRequestId)
                ?? throw new NotFoundException($"Friend request not found, Friend Request ID: {friendRequestId}");

            if (friendRequest.ReceiverId != _currentUserId)
            {
                throw new ForbiddenException("You do not have permission to accept this friend request.");
            }

            _friendRequestsRepository.DeleteFriendRequest(friendRequest);

            Friendship friendship = new Friendship()
            {
                ReceiverId = friendRequest.ReceiverId,
                SenderId = friendRequest.SenderId
            };

            await _friendshipsRepository.AddFriendshipAsync(friendship);

            if (!await _friendRequestsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Failed to save added friendship to database.");
            }
        }

        // Deletes a friendship from database
        public async Task DeleteFriendship(Guid userId)
        {
            Friendship friendship = await _friendshipsRepository.GetFriendshipByIdsAsync(userId, _currentUserId)
                ?? throw new NotFoundException($"Friendship not found, User ID: {userId} | Current User ID: {_currentUserId}");

            _friendshipsRepository.DeleteFriendship(friendship);

            if (!await _friendshipsRepository.IsSavedAsync())
            {
                throw new DbSavingFailedException("Faield to save friendship deletion to database.");
            }
        }

        // Deletes all friendships that contain user id
        public async Task DeleteFriendshipsByUserAsync()
        {
            int deleted = _friendshipsRepository.DeleteFriendshipsByUser(_currentUserId);

            if (deleted > 0)
            {
                if (!await _friendshipsRepository.IsSavedAsync())
                {
                    throw new DbSavingFailedException("Failed to save friendships deletion to the database.");
                }
            }
        }
    }
}
