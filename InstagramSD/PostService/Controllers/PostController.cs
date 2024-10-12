using Microsoft.AspNetCore.Mvc;
using PostService.Models;
using PostService.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PostService.Controllers
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

        [HttpPost("Upload")]
        public async Task<ActionResult> CreatePost([FromForm] UploadRequest uploadRequest)
        {
            return Ok(await _postService.CreatePost(uploadRequest));
        }

        [HttpPost("GetPostInformation")]
        public async Task<ActionResult> GetPostInformationByPostIds(List<Guid> postIds)
        {
            return Ok(await _postService.GetPostInformationByPostIds(postIds));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(Guid id)
        {
            var result = await _postService.DeletePost(id);
            if (!result)
            {
                return BadRequest($"{id} not found");
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAllPost()
        {
            var result = await _postService.DeleteAllPost();
            if (result == 0)
            {
                return BadRequest($"Could not delete anything");
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPost()
        {
            var result = await _postService.GetAllPost();
            if (result.Count == 0)
            {
                return BadRequest($"Could not get anything");
            }

            return Ok(result);
        }

    }
}

