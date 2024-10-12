using System;
using FriendService.Contracts;
using FriendService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FriendService.Controllers
{
	[ApiController]
    [Route("[controller]")]
    public class FriendController : ControllerBase
	{
        private readonly IFriendService _friendService;

        public FriendController(IFriendService friendService)
		{
            _friendService = friendService;
        }

        [HttpPost("Follow")]
        public async Task<ActionResult> FollowUser(FollowRequest followRequest)
        {
            var result = await _friendService.FollowFriend(followRequest);
            return Ok(result);
        }

        [HttpPost("Unfollow")]
        public async Task<ActionResult> UnfollowUser(UnfollowRequest unfollowRequest)
        {
            var result = await _friendService.UnfollowFriend(unfollowRequest);
            return Ok(result);
        }

    }
}

