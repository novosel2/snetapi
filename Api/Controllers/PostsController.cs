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


        // GET: /api/posts/popular-feed/popular?page=0

        [HttpGet("popular-feed")]
        public async Task<OkObjectResult> GetPopularFeed(int page)
        {
            List<PostResponse> postResponses = await _postsService.GetPopularFeedAsync(page);

            return Ok(postResponses);
        }


        // GET: /api/posts/your-feed?page=1

        [HttpGet("your-feed")]
        public async Task<OkObjectResult> GetYourFeed(int page)
        {
            List<PostResponse> postResponses = await _postsService.GetYourFeedAsync(page);

            return Ok(postResponses);
        }


        // GET: /api/posts/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpGet("{postId}")]
        public async Task<OkObjectResult> GetPostById(Guid postId)
        {
            PostResponse postResponse = await _postsService.GetPostByIdAsync(postId);

            return Ok(postResponse);
        }


        // GET: /api/posts/user/31faddd4-c910-45c2-a68b-bf67b5abaa77?page=0

        [HttpGet("user/{userId}")]
        public async Task<OkObjectResult> GetPostsByUserId(Guid userId, int page)
        {
            List<PostResponse> postResponses = await _postsService.GetPostsByUserIdAsync(userId, page);

            return Ok(postResponses);
        }


        // GET: /api/posts/username/novosel2?page=2

        [HttpGet("username/{username}")]
        public async Task<OkObjectResult> GetPostsByUsername(string username, int page)
        {
            List<PostResponse> postResponses = await _postsService.GetPostsByUsernameAsync(username, page);

            return Ok(postResponses);
        }

        // POST: /api/posts/add-post

        [HttpPost("add-post")]
        public async Task<OkObjectResult> AddPost([FromForm]PostAddRequest postAddRequest)
        {
            PostResponse postResponse = await _postsService.AddPostAsync(postAddRequest);

            return Ok(postResponse);
        }


        // PUT: /api/posts/update-post/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpPut("update-post/{postId}")]
        public async Task<OkObjectResult> UpdatePost(Guid postId, [FromForm]PostUpdateRequest postUpdateRequest)
        {
            PostResponse postResponse = await _postsService.UpdatePostAsync(postId, postUpdateRequest);

            return Ok(postResponse);
        }


        // DELETE: /api/posts/delete-post/31faddd4-c910-45c2-a68b-bf67b5abaa77

        [HttpDelete("delete-post/{postId}")]
        public async Task<OkResult> DeletePost(Guid postId)
        {
            await _postsService.DeletePostAsync(postId);

            return Ok();
        }
    }
}
