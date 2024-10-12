using Domain.EventMessages;
using Microsoft.Extensions.Options;
using PostProcessor.Repositories;
using PostProcessor.Settings;

namespace PostProcessor.Services
{
	public class ProcessPostCreatedService : IProcessPostCreatedService
	{
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRedisCacheRepository _redisCacheRepository;
        private readonly RankServiceHttpClientSettings _rankServiceHttpClientSettings;

        public ProcessPostCreatedService(
            IHttpClientFactory httpClientFactory,
            IOptions<RankServiceHttpClientSettings> rankServiceHttpClientSettings,
            IRedisCacheRepository redisCacheRepository)
		{
            _httpClientFactory = httpClientFactory;
            _redisCacheRepository = redisCacheRepository;
            _rankServiceHttpClientSettings = rankServiceHttpClientSettings.Value;
        }

        public async Task Process(PostCreated postCreated)
        {
            Console.WriteLine($"Processing postcreated in PostProcessorService : {postCreated.PostId}");

            //call rankservice api
            //getListFriensToSharePostByUserId
            var listOfFriends = await getListFriensToSharePostByUserId(postCreated.User.UserId);

            //Update the list in the Redis DB
            await addPostIdInListOfFriends(postCreated.PostId, listOfFriends);
        }

        private async Task addPostIdInListOfFriends(Guid postId, List<Guid> listOfFriends)
        {
            await _redisCacheRepository.updatePostsIdsInUserList(postId, listOfFriends);
        }

        private async Task<List<Guid>> getListFriensToSharePostByUserId(Guid userId)
        {
            using var httpClient = _httpClientFactory.CreateClient(_rankServiceHttpClientSettings.Name);
            var listOfFriends = await httpClient.GetFromJsonAsync<Guid[]>($"/Rank?userId={userId}");
            return listOfFriends?.ToList() ?? null;
        }

        //private async Task<List<Guid>> getListFriensToSharePostByUserId(Guid userId)
        //{
        //    var listOfFriends = new List<Guid>();
        //    listOfFriends.Add(Guid.Parse("12c3f731-1ed5-45fa-b76a-d982f6aaecd6"));
        //    listOfFriends.Add(Guid.Parse("22c3f731-1ed5-45fa-b76a-d982f6aaecd6"));
        //    listOfFriends.Add(Guid.Parse("32c3f731-1ed5-45fa-b76a-d982f6aaecd6"));

        //    return listOfFriends;
        //}
    }
}

