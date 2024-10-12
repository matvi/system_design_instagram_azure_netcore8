using System;
using Domain.Dtos;
using Domain.Models;
using FeedService.Models;

namespace FeedService.Factories
{
	public interface IFeedResponseFactory
	{
        public FeedResponse GetFeedResponseWithError(string error);
        public FeedResponse GetFeedResponseSuccess();
        public FeedResponse GetFeedResponseSuccessWithPosts(List<PostDto> posts);
    }
}

