
using Common.ServiceBus;
using Domain.EventMessages;
using Domain.Models;
using LikeService.Contracts;
using LikeService.Repositories;

namespace LikeService.Services
{
	public class LikeService : ILikeService
	{
        private readonly ILikeRepository _likeRepository;
        private readonly IEventBus _eventBus;

        public LikeService(
            ILikeRepository likeRepository,
            IEventBus eventBus)
		{
            _likeRepository = likeRepository;
            _eventBus = eventBus;
        }

        public async Task<LikePostResponse> LikePost(LikePostRequest likePostRequest)
        {
            var like = new Like
            {
                UserId = likePostRequest.UserId,
                PostId = likePostRequest.PostId
            };
            var response = await _likeRepository.AddLikeAsyc(like);
            if (response == 0)
            {
                return new LikePostResponse
                {
                    ErrorMessage = $"Could not like postId {likePostRequest.PostId}",
                    IsSuccess = false
                };
            }

            //send servie bus
            _eventBus.PublishMessage<LikeCreated>(new LikeCreated
            {
                LikeId = like.LikeId,
                PostId = like.PostId,
                UserId = like.UserId
            }) ;

            return new LikePostResponse
            {
                Like = like
            };
        }

        public async Task<UnlikePostResponse> UnlikePost(UnlikePostRequest unlikePostRequest)
        {
            var like = await _likeRepository.UnlikeAsync(unlikePostRequest.UserId, unlikePostRequest.PostId);

            if (like is null)
            {
                return new UnlikePostResponse
                {
                    IsSuccess = false,
                    ErrorMessage = $"Could not find like from Userid {unlikePostRequest.UserId} and postId {unlikePostRequest.PostId}"
                };
            }

            _eventBus.PublishMessage<UnlikeCreated>(new UnlikeCreated
            {
                LikeId = like.LikeId,
                PostId = like.PostId,
                UserId = like.UserId
            });

            return new UnlikePostResponse();
        }
      
    }
}

