using System;
using Common.Interfaces;
using Domain.Models;

namespace LikeService.Contracts
{
	public class LikePostResponse : ResponseBase
	{
		public Like Like { get; set; }
	}
}

