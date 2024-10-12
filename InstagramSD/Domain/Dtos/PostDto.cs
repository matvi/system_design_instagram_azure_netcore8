using System;
using Domain.Models;

namespace Domain.Dtos
{
    public class PostDto
    {
        public Guid PostId { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public string PostText { get; set; }
        public int? Likes { get; set; }
        public User User { get; set; }
    }
}

