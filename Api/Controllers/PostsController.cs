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

        public PostsController(IPostsService postsService)
        {
            _postsService = postsService;
        }


        // GET: /api/posts?page=0

        [HttpGet]
        public async Task<IActionResult> GetPosts(int page)
        {
            List<PostResponse> postResponses = await _postsService.GetPostsAsync(page);

            return Ok(postResponses);
        }


        // GET: /api/posts/your-feed?page=0

        [HttpGet("your-feed")]
        public async Task<IActionResult> GetYourFeed(int page)
        {
            List<PostResponse> postResponses = await _postsService.GetYourFeedAsync(page);

            return Ok(postResponses);
        }


        // GET: /api/posts/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById(Guid postId)
        {
            PostResponse postResponse = await _postsService.GetPostByIdAsync(postId);

            return Ok(postResponse);
        }


        // GET: /api/posts/user/31faddd4-c910-45c2-a68b-bf67b5abaa77?page=0

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPostsByUserId(Guid userId, int page)
        {
            List<PostResponse> postResponses = await _postsService.GetPostsByUserIdAsync(userId, page);

            return Ok(postResponses);
        }


        // POST: /api/posts/add-post

        [HttpPost("add-post")]
        public async Task<IActionResult> AddPost([FromForm]PostAddRequest postAddRequest)
        {
            await _postsService.AddPostAsync(postAddRequest);

            return Ok();
        }


        // PUT: /api/posts/update-post/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpPut("update-post/{postId}")]
        public async Task<IActionResult> UpdatePost(Guid postId, [FromForm]PostUpdateRequest postUpdateRequest)
        {
            await _postsService.UpdatePostAsync(postId, postUpdateRequest);

            return Ok();
        }


        // DELETE: /api/posts/delete-post/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpDelete("delete-post/{postId}")]
        public async Task<IActionResult> DeletePost(Guid postId)
        {
            await _postsService.DeletePostAsync(postId);

            return Ok();
        }
    }
}
