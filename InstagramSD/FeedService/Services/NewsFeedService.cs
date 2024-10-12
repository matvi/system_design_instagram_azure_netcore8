using System.Text;
using System.Text.Json;
using Common.Extensions;
using Domain.Dtos;
using FeedService.Factories;
using FeedService.Models;
using FeedService.Settings;
using Microsoft.Extensions.Options;

namespace FeedService.Services
{
    public class NewsFeedService : INewsFeedService
	{
        private readonly PostHttpClientSettings _postHttpClientSettings;
        private readonly PostProcessorHttpClientSettings _postProcessorHttpClientSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IFeedResponseFactory _feedResponseFactory;
        

        public NewsFeedService(
			IOptions<PostHttpClientSettings> postHttpClientSettings,
            IOptions<PostProcessorHttpClientSettings> postProcessorHttpClientSettings,
			IHttpClientFactory httpClientFactory,
            IFeedResponseFactory feedResponseFactory)
		{
            _postHttpClientSettings = postHttpClientSettings.Value;
            _postProcessorHttpClientSettings = postProcessorHttpClientSettings.Value;
            _httpClientFactory = httpClientFactory;
            _feedResponseFactory = feedResponseFactory;
        }

		public async Task<FeedResponse> GetNewFeedByUserIdAsync(Guid userId)
        {
            var postIds = await GetPostIdsByUserIdAsync(userId);
            if (NotPostFound(postIds))
            {
                return _feedResponseFactory.GetFeedResponseWithError($"Not posts found for userId {userId}");
            }

            var posts = await GetPostInformationByPostIdsAsync(postIds);

            return _feedResponseFactory.GetFeedResponseSuccessWithPosts(posts);
        }

        private static bool NotPostFound(List<string>? postIds)
        {
            return postIds is null || postIds.Count == 0;
        }

        private async Task<List<PostDto>> GetPostInformationByPostIdsAsync(List<string> postsIds)
		{
            var httpClient = _httpClientFactory.CreateClient(_postHttpClientSettings.Name);
            var jsonString = JsonSerializer.Serialize(postsIds);
            var payload = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var httpResponseMessage = await httpClient.PostAsync("/post/GetPostInformation", payload);
            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            var posts = response.DeserializeCustom<List<PostDto>>();

            return posts;
		}

        private async Task<List<string>> GetPostIdsByUserIdAsync(Guid userId)
		{
            var httpClient = _httpClientFactory.CreateClient(_postProcessorHttpClientSettings.Name);
            var httpResponseMessage = await httpClient.GetAsync($"/post?userId={userId}");
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();

            var postsIds = JsonSerializer.Deserialize<List<string>>(responseString);
            return postsIds;
		}
    }
}

