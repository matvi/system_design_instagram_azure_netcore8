using System;
using Microsoft.AspNetCore.Mvc;
using PostProcessor.Services;

namespace PostProcessor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
	{
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<ActionResult> GetPostsListByUserId([FromQuery] Guid userId)
        {
            var listOfPosts = await _postService.GetPostsListByUserIdAsync(userId);
            return Ok(listOfPosts);
        }
	}
}

