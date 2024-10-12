using System;
using Common.Interfaces;
using Domain.Dtos;
using Domain.Models;

namespace FeedService.Models
{
	public class FeedResponse : ResponseBase
	{
		public List<PostDto> Posts { get; set; }
	}
}

