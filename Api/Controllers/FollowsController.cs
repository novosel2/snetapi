using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/follows")]
    public class FollowsController : ControllerBase
    {
        private readonly IFollowsService _followsService;

        public FollowsController(IFollowsService followsService)
        {
            _followsService = followsService;
        }


        // POST: api/follows/add-follow/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpPost("add-follow/{userId}")]
        public async Task<OkResult> AddFollow(Guid userId)
        {
            await _followsService.AddFollowAsync(userId);

            return Ok();
        }


        // DELETE: api/follows/delete-follow/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpDelete("unfollow/{userId}")]
        public async Task<OkResult> DeleteFollow(Guid userId)
        {
            await _followsService.DeleteFollowAsync(userId);

            return Ok();
        }
    }
}
