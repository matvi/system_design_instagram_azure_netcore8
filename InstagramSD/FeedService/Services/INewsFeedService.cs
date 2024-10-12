using FeedService.Models;

namespace FeedService.Services
{
    public interface INewsFeedService
	{
        Task<FeedResponse> GetNewFeedByUserIdAsync(Guid userId);
    }
}

