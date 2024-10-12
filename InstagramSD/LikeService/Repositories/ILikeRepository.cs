using System;
using Domain.Models;

namespace LikeService.Repositories
{
	public interface ILikeRepository
	{
		public Task<int> AddLikeAsyc(Like like);
		public Task<Like> UnlikeAsync(Guid userId, Guid postId);
    }
}

