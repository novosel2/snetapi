using Core.Data.Dto.PostDto;
using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            Guid _userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value.ToString());

            await _postsService.AddPostAsync(postAddRequest, _userId);

            return Ok("Post successfully added.");
        }


        // PUT: /api/posts/update-post/31faddd4-c910-45c2-a68b-bf67b5abaa77/

        [HttpPut("update-post/{postId}")]
        public async Task<IActionResult> UpdatePost(Guid postId, PostUpdateRequest postUpdateRequest)
        {
            Guid _userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value.ToString());

            await _postsService.UpdatePostAsync(postId, postUpdateRequest, _userId);

            return Ok("Post successfully updated.");
        }


        // DELETE: /api/posts/delete-post/31faddd4-c910-45c2-a68b-bf67b5abaa77/

        [HttpDelete("delete-post/{postId}")]
        public async Task<IActionResult> DeletePost(Guid postId)
        {
            Guid _userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value.ToString());

            await _postsService.DeletePostAsync(postId, _userId);

            return Ok("Post successfully deleted.");
        }
    }
}
