using System;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using RankService.Services;

namespace RankService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RankController : ControllerBase
	{
        private readonly IRankService _rankService;

        public RankController(IRankService rankService)
		{
            this._rankService = rankService;
        }

		[HttpGet]
		public async Task<ActionResult> GetListFriendsToSharePostByUserId(Guid userId)
		{
			var listOfFriends = await _rankService.GetListOfFriendsToSharePostByUserId(userId);
			if (listOfFriends is null)
			{
				return NotFound($"userId {User} not found");
			}

			return Ok(listOfFriends);
		}

		[HttpPost]
		public async Task<ActionResult> InsertManualRank(List<Rank> rankList)
		{
			var result = await _rankService.InsertManualRank(rankList);
			if(result == 0)
			{
				return BadRequest("Could not insert rank");
			}

            return Ok(rankList);
		}

		[HttpDelete]
		public async Task<ActionResult> DeleteAll()
		{
			var result = await _rankService.DeleteAll();
			if (!result)
			{
				return BadRequest("could not delete");
			}

			return Ok();
		}
	}
}

