using System;
namespace Domain.Models
{
	public class Like
	{
		public Guid LikeId { get; set; } = Guid.NewGuid();
		public Guid UserId { get; set; }
		public Guid PostId { get; set; } //partitionkey
	}
}

