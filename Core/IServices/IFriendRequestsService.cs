﻿using Core.Data.Dto.FriendsDto;
using Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IFriendRequestsService
    {
        /// <summary>
        /// Get all sent friend requests
        /// </summary>
        /// <returns>List of friend request responses</returns>
        public Task<List<FriendRequestResponse>> GetSentFriendRequestsAsync();

        /// <summary>
        /// Get all received friend requests
        /// </summary>
        /// <returns>List of friend request responses</returns>
        public Task<List<FriendRequestResponse>> GetReceivedFriendRequestsAsync();

        /// <summary>
        /// Adds a friend request
        /// </summary>
        /// <param name="recieverUserId">User id you want to send friend request to</param>
        public Task AddFriendRequestAsync(Guid recieverUserId);

        /// <summary>
        /// Deletes a friend request from database
        /// </summary>
        /// <param name="friendRequestId">Friend request id you want to delete</param>
        public Task DeleteFriendRequestAsync(Guid friendRequestId);
    }
}