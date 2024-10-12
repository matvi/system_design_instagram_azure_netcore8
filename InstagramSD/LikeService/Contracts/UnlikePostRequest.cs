using System;
namespace LikeService.Contracts
{
	public class UnlikePostRequest
	{
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
    }
}

