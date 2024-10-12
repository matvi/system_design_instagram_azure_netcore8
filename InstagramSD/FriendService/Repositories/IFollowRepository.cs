using System;
using Domain.Models;

namespace FriendService.Repositories
{
	public interface IFollowRepository
	{
        Task<bool> UnfollowUserAsync(Guid userId, Guid unfollowUser);
        Task<int> FollowUserAsync(Follow follow);
    }
}

