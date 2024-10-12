using System;
using Common.ServiceBus;
using Domain.EventMessages;
using Domain.Models;
using FriendService.Contracts;
using FriendService.Repositories;

namespace FriendService.Services
{
	public class FriendService : IFriendService
	{
        private readonly IFollowRepository _followRepository;
        private readonly IEventBus _eventBus;

        public FriendService(
            IFollowRepository followRepository,
            IEventBus eventBus)
		{
            _followRepository = followRepository;
            _eventBus = eventBus;
        }

        public async Task<FollowResponse> FollowFriend(FollowRequest followRequest)
        {
            //save in DB
            var follow = new Follow
            {
                UserId = followRequest.UserId,
                FollowUserId = followRequest.FollowUserId,
                FollowType = Domain.Enums.FollowType.Friend,
                FollowId = Guid.NewGuid()
            };

            var response = await _followRepository.FollowUserAsync(follow);
            if(response == 0)
            {
                return new FollowResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Could not add the follower"
                };
            }

            //sent event
            _eventBus.PublishMessage<FollowerCreated>(new FollowerCreated
            {
                UserId = followRequest.UserId,
                FollowUserId = followRequest.FollowUserId
            });

            return new FollowResponse
            {
                Follow = follow
            };
        }

        public async Task<UnfollowResponse> UnfollowFriend(UnfollowRequest unfollowRequest)
        {
            var responseSuccess = await _followRepository.UnfollowUserAsync(unfollowRequest.UserId, unfollowRequest.UnfollowUserId);

            if (responseSuccess)
            {
                //sent event
                _eventBus.PublishMessage<UnfollowedCreated>(new UnfollowedCreated
                {
                    UserId = unfollowRequest.UserId,
                    UnfollowUserId = unfollowRequest.UnfollowUserId
                });
            }

            return new UnfollowResponse
            {
                IsSuccess = responseSuccess
            };
        }
    }
}

