using System;
using FriendService.Contracts;

namespace FriendService.Services
{
	public interface IFriendService
	{
        Task<FollowResponse> FollowFriend(FollowRequest followRequest);
        Task<UnfollowResponse> UnfollowFriend(UnfollowRequest unfollowRequest);
    }
}

