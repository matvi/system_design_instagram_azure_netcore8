using System;
using Domain.interfaces;
using Domain.Models;

namespace Domain.EventMessages
{
	public class PostCreated : IInstagramEvent
	{
        public Guid PostId { get; set; }
        public string ImageUrl { get; set; }
        public string ImageFileName { get; set; }
        public string PostTitle { get; set; }
        public string PostText { get; set; }
        public User User { get; set; }
        public string EventType { get; set; } = Enums.EventType.PostCreated.ToString();
    }
    
}

