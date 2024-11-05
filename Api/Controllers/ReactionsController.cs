using Core.Data.Dto.PostDto;
using Core.Enums;
using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/reactions/")]
    public class ReactionsController : ControllerBase
    {
        private readonly IPostReactionsService _postReactionsService;

        public ReactionsController(IPostReactionsService postReactionsService)
        {
            _postReactionsService = postReactionsService;
        }



        // POST: /api/reactions/posts/add/31faddd4-c910-45c2-a68b-bf67b5abaa77/

        [HttpPost("posts/add/{postId}")]
        public async Task<IActionResult> AddPostReaction(Guid postId, ReactionType reaction)
        {
            await _postReactionsService.AddPostReactionAsync(postId, reaction);

            return Ok();
        }

        // PUT: /api/reactions/posts/update/31faddd4-c910-45c2-a68b-bf67b5abaa77/

        [HttpPut("posts/update/{postId}")]
        public async Task<IActionResult> UpdatePostReaction(Guid postId)
        {
            await _postReactionsService.UpdatePostReaction(postId);

            return Ok();
        }

        // DELETE /api/reactions/posts/remove/31faddd4-c910-45c2-a68b-bf67b5abaa77/

        [HttpDelete("posts/delete/{postId}")]
        public async Task<IActionResult> DeletePostReaction(Guid postId)
        {
            await _postReactionsService.DeletePostReaction(postId);

            return Ok();
        }
    }
}
