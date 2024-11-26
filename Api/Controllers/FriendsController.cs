using Core.Data.Dto.FriendsDto;
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
        public async Task<IActionResult> GetSentFriendRequests()
        {
            List<FriendRequestResponse> friendRequestResponses = await _friendRequestsService.GetSentFriendRequestsAsync();

            return Ok(friendRequestResponses);
        }


        // GET: /api/friends/friend-requests/received

        [HttpGet("friend-requests/received")]
        public async Task<IActionResult> GetReceivedFriendRequests()
        {
            List<FriendRequestResponse> friendRequestResponses = await _friendRequestsService.GetReceivedFriendRequestsAsync();

            return Ok(friendRequestResponses);
        }


        // POST: /api/friends/friend-requests/send

        [HttpPost("friend-requests/send/{recieverUserId}")]
        public async Task<IActionResult> SendFriendRequest(Guid recieverUserId)
        {
            await _friendRequestsService.AddFriendRequestAsync(recieverUserId);

            return Ok();
        }


        // POST: /api/friends/friend-requests/accept/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpPost("friend-requests/accept/{friendRequestId}")]
        public async Task<IActionResult> AcceptFriendRequest(Guid friendRequestId)
        {
            await _friendshipsService.AddFriendshipAsync(friendRequestId);

            return Ok();
        }


        // DELETE: /api/friends/friend-requests/decline/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpPost("friend-requests/decline/{friendRequestId}")]
        public async Task<IActionResult> DeclineFriendRequest(Guid friendRequestId)
        {
            await _friendRequestsService.DeleteFriendRequestAsync(friendRequestId);

            return Ok();
        }


        // GET: /api/friends/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFriendshipsByUserId(Guid userId)
        {
            List<FriendshipResponse> friendshipResponses = await _friendshipsService.GetFriendshipsByUserIdAsync(userId);

            return Ok(friendshipResponses);
        }


        // DELETE: /api/friends/delete/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpPost("delete/{userId}")]
        public async Task<IActionResult> DeleteFriendship(Guid userId)
        {
            await _friendshipsService.DeleteFriendship(userId);

            return Ok();
        }
    }
}
