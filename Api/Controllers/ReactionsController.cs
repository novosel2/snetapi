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
        private readonly ICommentReactionsService _commentReactionsService;

        public ReactionsController(IPostReactionsService postReactionsService, ICommentReactionsService commentReactionsService)
        {
            _postReactionsService = postReactionsService;
            _commentReactionsService = commentReactionsService;
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


        // POST: /api/reactions/comment/add/31faddd4-c910-45c2-a68b-bf67b5abaa77/

        [HttpPost("comments/add/{commentId}")]
        public async Task<IActionResult> AddCommentReaction(Guid commentId, ReactionType reaction)
        {
            await _commentReactionsService.AddCommentReactionAsync(commentId, reaction);

            return Ok();
        }


        // PUT: /api/reactions/comment/update/31faddd4-c910-45c2-a68b-bf67b5abaa77/

        [HttpPut("comments/update/{commentId}")]
        public async Task<IActionResult> UpdateCommentReaction(Guid commentId)
        {
            await _commentReactionsService.UpdateCommentReaction(commentId);

            return Ok();
        }


        // DELETE /api/reactions/comment/remove/31faddd4-c910-45c2-a68b-bf67b5abaa77/

        [HttpDelete("comments/delete/{commentId}")]
        public async Task<IActionResult> DeleteCommentReaction(Guid commentId)
        {
            await _commentReactionsService.DeleteCommentReaction(commentId);

            return Ok();
        }
    }
}
