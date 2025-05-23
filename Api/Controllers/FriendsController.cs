﻿using Core.Data.Dto.FriendsDto;
using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/friends/")]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendRequestsService _friendRequestsService;
        private readonly IFriendshipsService _friendshipsService;

        public FriendsController(IFriendRequestsService friendRequestsService, IFriendshipsService friendshipsService)
        {
            _friendRequestsService = friendRequestsService;
            _friendshipsService = friendshipsService;
        }


        // GET: /api/friends/friend-requests/sent

        [HttpGet("friend-requests/sent")]
        public async Task<OkObjectResult> GetSentFriendRequests()
        {
            List<FriendRequestResponse> friendRequestResponses = await _friendRequestsService.GetSentFriendRequestsAsync();

            return Ok(friendRequestResponses);
        }


        // GET: /api/friends/friend-requests/received

        [HttpGet("friend-requests/received")]
        public async Task<OkObjectResult> GetReceivedFriendRequests()
        {
            List<FriendRequestResponse> friendRequestResponses = await _friendRequestsService.GetReceivedFriendRequestsAsync();

            return Ok(friendRequestResponses);
        }


        // POST: /api/friends/friend-requests/send

        [HttpPost("friend-requests/send/{receiverUserId}")]
        public async Task<OkResult> SendFriendRequest(Guid recieverUserId)
        {
            await _friendRequestsService.AddFriendRequestAsync(recieverUserId);

            return Ok();
        }


        // POST: /api/friends/friend-requests/accept/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpPost("friend-requests/accept/{userId}")]
        public async Task<OkResult> AcceptFriendRequest(Guid userId)
        {
            await _friendRequestsService.AddFriendRequestAsync(userId);

            return Ok();
        }


        // DELETE: /api/friends/friend-requests/decline/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpDelete("friend-requests/decline/{userId}")]
        public async Task<OkResult> DeclineFriendRequest(Guid userId)
        {
            await _friendRequestsService.DeleteFriendRequestAsync(userId);

            return Ok();
        }


        // GET: /api/friends/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpGet("{userId}")]
        public async Task<OkObjectResult> GetFriendshipsByUserId(Guid userId)
        {
            List<FriendshipResponse> friendshipResponses = await _friendshipsService.GetFriendshipsByUserIdAsync(userId);

            return Ok(friendshipResponses);
        }


        // DELETE: /api/friends/delete/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpDelete("delete/{userId}")]
        public async Task<OkResult> DeleteFriendship(Guid userId)
        {
            await _friendshipsService.DeleteFriendship(userId);

            return Ok();
        }
    }
}
