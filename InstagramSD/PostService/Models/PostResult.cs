using System;
namespace PostService.Models
{
	public class PostResult : Result
	{
        public string ImageUrl { get; set; }
        public Guid PostId { get; internal set; }
    }
}

