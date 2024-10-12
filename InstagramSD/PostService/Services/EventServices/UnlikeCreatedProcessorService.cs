using Domain.EventMessages;
using PostService.Repositories;

namespace PostService.Services
{
    public class UnlikeCreatedProcessorService : IUnlikeCreatedProcessorService
	{
        private readonly IPostRepository _postRepository;

        public UnlikeCreatedProcessorService(IPostRepository postRepository)
		{
            _postRepository = postRepository;
        }

        public async Task UpdateLike(UnlikeCreated unlikeCreated)
        {
            await _postRepository.UpdateUnlikePost(unlikeCreated.PostId);
        }
	}
}

