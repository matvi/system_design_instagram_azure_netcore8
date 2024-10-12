using System;
using Domain.interfaces;

namespace Domain.EventMessages
{
	public class LikeCreated : IInstagramEvent
    {
        public Guid LikeId { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public string EventType { get; set; } = Enums.EventType.LikeCreated.ToString();
    }
}

