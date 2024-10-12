using PostProcessor.Repositories;

namespace PostProcessor.Services
{
    public class PostService : IPostService
	{
        private readonly IRedisCacheRepository _redisCacheRepository;

        public PostService(IRedisCacheRepository redisCacheRepository)
		{
            _redisCacheRepository = redisCacheRepository;
        }

        public async Task<List<Guid>> GetPostsListByUserIdAsync(Guid userId)
        {
            var key = $"userId:userId:{userId.ToString()}";
            var listOfPosts = await _redisCacheRepository.GetCasheDataListString(key);
            var listOfPostsGuid = listOfPosts.Select(postId => Guid.Parse(postId)).ToList();
            return listOfPostsGuid;
        }
    }
}

