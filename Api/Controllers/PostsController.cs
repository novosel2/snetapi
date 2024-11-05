using Core.Data.Dto.PostDto;
using Core.Enums;
using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/posts/")]
    public class PostsController : ControllerBase
    {
        private readonly IPostsService _postsService;
        private readonly IPostReactionsService _postReactionsService;

        public PostsController(IPostsService postsService, IPostReactionsService postReactionsService)
        {
            _postsService = postsService;
            _postReactionsService = postReactionsService;
        }


        // GET: /api/posts/

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            List<PostResponse> postResponses = await _postsService.GetPostsAsync();

            return Ok(postResponses);
        }


        // GET: /api/posts/31faddd4-c910-45c2-a68b-bf67b5abaa77/

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById(Guid postId)
        {
            PostResponse postResponse = await _postsService.GetPostByIdAsync(postId);

            return Ok(postResponse);
        }


        // GET: /api/posts/user/31faddd4-c910-45c2-a68b-bf67b5abaa77/

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPostsByUserId(Guid userId)
        {
            List<PostResponse> postResponses = await _postsService.GetPostsByUserIdAsync(userId);

            return Ok(postResponses);
        }


        // POST: /api/posts/add-post/

        [HttpPost("add-post")]
        public async Task<IActionResult> AddPost(PostAddRequest postAddRequest)
        {
            await _postsService.AddPostAsync(postAddRequest);

            return Ok();
        }


        // PUT: /api/posts/update-post/31faddd4-c910-45c2-a68b-bf67b5abaa77/

        [HttpPut("update-post/{postId}")]
        public async Task<IActionResult> UpdatePost(Guid postId, PostUpdateRequest postUpdateRequest)
        {
            await _postsService.UpdatePostAsync(postId, postUpdateRequest);

            return Ok();
        }


        // DELETE: /api/posts/delete-post/31faddd4-c910-45c2-a68b-bf67b5abaa77/

        [HttpDelete("delete-post/{postId}")]
        public async Task<IActionResult> DeletePost(Guid postId)
        {
            await _postsService.DeletePostAsync(postId);

            return Ok();
        }


        // POST: /api/posts/reactions/add/31faddd4-c910-45c2-a68b-bf67b5abaa77/

        [HttpPost("reactions/add/{postId}")]
        public async Task<IActionResult> AddPostReaction(Guid postId, ReactionType reaction)
        {
            await _postReactionsService.AddPostReactionAsync(postId, reaction);

            return Ok();
        }

        // PUT: /api/posts/reactions/update/31faddd4-c910-45c2-a68b-bf67b5abaa77/

        [HttpPut("reactions/update/{postId}")]
        public async Task<IActionResult> UpdatePostReaction(Guid postId)
        {
            await _postReactionsService.UpdatePostReaction(postId);

            return Ok();
        }

        // DELETE /api/posts/reactions/remove/31faddd4-c910-45c2-a68b-bf67b5abaa77/

        [HttpDelete("reactions/delete/{postId}")]
        public async Task<IActionResult> DeletePostReaction(Guid postId)
        {
            await _postReactionsService.DeletePostReaction(postId);

            return Ok();
        }
    }
}
