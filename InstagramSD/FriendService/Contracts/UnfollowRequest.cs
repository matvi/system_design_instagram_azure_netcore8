using System;
namespace FriendService.Contracts
{
	public class UnfollowRequest
	{
        public Guid UserId { get; set; }
        public Guid UnfollowUserId { get; set; }
    }
}

