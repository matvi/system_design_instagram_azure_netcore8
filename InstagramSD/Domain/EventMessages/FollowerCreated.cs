using System;
using Domain.interfaces;

namespace Domain.EventMessages
{
    public class FollowerCreated : IInstagramEvent
    {
        public Guid UserId { get; set; }
        public Guid FollowUserId { get; set; }
        public string EventType { get; set; } = Enums.EventType.FollowCreated.ToString();
    }
}

