using System;
using Domain.Enums;

namespace Domain.Models
{
	public class Follow
	{
        public Guid FollowId { get; set; }
        public Guid UserId { get; set; }
        public Guid FollowUserId { get; set; }
        public FollowType FollowType { get; set; }
    }
}

