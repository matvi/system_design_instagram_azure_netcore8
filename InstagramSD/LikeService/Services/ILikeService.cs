using System;
using LikeService.Contracts;

namespace LikeService.Services
{
	public interface ILikeService
	{
        public Task<LikePostResponse> LikePost(LikePostRequest likePostRequest);
        public Task<UnlikePostResponse> UnlikePost(UnlikePostRequest unlikePostRequest);
    }
}

