using System;
using Domain.Dtos;
using FeedService.Models;

namespace FeedService.Factories
{
	public class FeedResponseFactory : IFeedResponseFactory
	{
		public FeedResponse GetFeedResponseWithError(string error)
		{
			return new FeedResponse
			{
				IsSuccess = false,
				ErrorMessage = error
			};
		}

        public FeedResponse GetFeedResponseSuccess()
        {
			return new FeedResponse();
        }

        public FeedResponse GetFeedResponseSuccessWithPosts(List<PostDto> posts)
        {
			return new FeedResponse
			{
				Posts = posts
			};
        }
    }
}

