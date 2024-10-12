using System;
using Domain.interfaces;

namespace Domain.EventMessages
{
    public class UnfollowedCreated : IInstagramEvent
    {
        public Guid UserId { get; set; }
        public Guid UnfollowUserId { get; set; }
        public string EventType { get; set; } = Enums.EventType.UnfollowCreated.ToString();
    }
}

