using System;
namespace FriendService.Contracts
{
	public class FollowRequest
	{
		public Guid UserId { get; set; }
		public Guid FollowUserId { get; set; }
	}
}

