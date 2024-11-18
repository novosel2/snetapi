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

        public FriendsController(IFriendRequestsService friendRequestsService)
        {
            _friendRequestsService = friendRequestsService;
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
    }
}
