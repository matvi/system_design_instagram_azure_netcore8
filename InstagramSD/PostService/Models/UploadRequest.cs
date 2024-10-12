using System;
namespace PostService.Models
{
	public class UploadRequest
	{
		public IFormFile file { get; set; }
		public string PostTitle { get; set; }
		public string PostText { get; set; }
		public Guid UserId { get; set; }
		//public string UserName { get; set; }
		//public string UserProfileImageUrl { get; set; }
    }
}

