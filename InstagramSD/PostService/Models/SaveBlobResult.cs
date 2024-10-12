using System;
namespace PostService.Models
{
	public class SaveBlobResult : Result
	{
        public string ImageUrl { get; set; }
        public string ImageUrlWithSasToken { get; set; }
        public string ImageFileName { get; internal set; }
    }
}

