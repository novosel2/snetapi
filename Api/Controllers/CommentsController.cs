using Core.Data.Dto.CommentDto;
using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/comments/")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }


        // POST: /api/comments/add/31faddd4-c910-45c2-a68b-bf67b5abaa77
        
        [HttpPost("add/{postId}")]
        public async Task<IActionResult> AddComment(Guid postId, CommentAddRequest commentAddRequest)
        {
            await _commentsService.AddCommentAsync(postId, commentAddRequest);

            return Ok();
        }


        // POST: /api/comments/add-reply/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpPost("add-reply/{commentId}")]
        public async Task<IActionResult> AddCommentReply(Guid commentId, CommentAddRequest commentAddRequest)
        {
            await _commentsService.AddCommentReplyAsync(commentId, commentAddRequest);

            return Ok();
        }


        // PUT: /api/comments/update/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpPut("update/{commentId}")]
        public async Task<IActionResult> UpdateComment(Guid commentId, CommentUpdateRequest commentUpdateRequest)
        {
            await _commentsService.UpdateCommentAsync(commentId, commentUpdateRequest);

            return Ok();
        }


        // DELETE: /api/comments/delete/31faddd4-c910-45c2-a68b-bf67b5abaa77
        [HttpDelete("delete/{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            await _commentsService.DeleteCommentAsync(commentId);

            return Ok();
        }
    }
}
