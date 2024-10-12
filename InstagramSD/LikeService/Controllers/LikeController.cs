using System;
using LikeService.Contracts;
using LikeService.Services;
using Microsoft.AspNetCore.Mvc;

namespace LikeService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LikeController : ControllerBase
	{
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
		{
            _likeService = likeService;
        }

        [HttpPost]
        public async Task<ActionResult> LikePost(LikePostRequest likePostRequest)
        {
            var response = await _likeService.LikePost(likePostRequest);
            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response);
        }

        [HttpPost("unlike")]
        public async Task<ActionResult> UnlikePost(UnlikePostRequest unlikePostRequest)
        {
            var response = await _likeService.UnlikePost(unlikePostRequest);
            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response);
        }
    }
}

