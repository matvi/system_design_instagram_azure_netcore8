using FeedService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FeedService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeedController: ControllerBase
	{
        private readonly INewsFeedService _newsFeedService;

        public FeedController(INewsFeedService newsFeedService)
		{
            _newsFeedService = newsFeedService;
        }

		[HttpGet("{userId}")]
		public async Task<ActionResult> GetNewsFedByUserId(Guid userId)
		{
            var feedResponse = await _newsFeedService.GetNewFeedByUserIdAsync(userId);
            return Ok(feedResponse);
        }
    }
}

