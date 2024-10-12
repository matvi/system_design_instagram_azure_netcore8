using System;
using Domain.EventMessages;
using PostService.Repositories;

namespace PostService.Services
{
    public class LikeCreatedProcessorService : ILikeCreatedProcessorService
    {
        private readonly IPostRepository _postRepository;

        public LikeCreatedProcessorService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task ProcessLikeEvent(LikeCreated likeCreated)
        {
            Console.WriteLine("Updating like in post");
            await _postRepository.UpdateLikePost(likeCreated.PostId);
        }
    }
}

