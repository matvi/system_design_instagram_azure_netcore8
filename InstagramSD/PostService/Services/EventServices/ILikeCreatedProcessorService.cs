using System;
using Domain.EventMessages;

namespace PostService.Services
{
	public interface ILikeCreatedProcessorService
	{
		Task ProcessLikeEvent(LikeCreated likeCreated);
	}
}

