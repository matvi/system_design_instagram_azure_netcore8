using System;
using Domain.Enums;
using Domain.Models;
using FriendService.Data;
using Microsoft.EntityFrameworkCore;

namespace FriendService.Repositories
{
	public class FollowRepository : IFollowRepository
	{
        private readonly FriendDbContext _friendDbContext;

        public FollowRepository(FriendDbContext friendDbContext)
		{
            _friendDbContext = friendDbContext;
        }

        public async Task<int> FollowUserAsync(Follow follow)
        {
            await _friendDbContext.Follows.AddAsync(follow);
            return await _friendDbContext.SaveChangesAsync();
        }

        public async Task<bool> UnfollowUserAsync(Guid userId, Guid unfollowUser)
        {
            var follow = await _friendDbContext.Follows.FirstOrDefaultAsync(
                f => f.FollowUserId == unfollowUser &&
                f.UserId == userId
                && f.FollowType == FollowType.Friend);//partitionKey

            if(follow is null)
            {
                return false;
            }

            _friendDbContext.Follows.Remove(follow);

            return (await _friendDbContext.SaveChangesAsync()) == 1;
        }
	}
}

