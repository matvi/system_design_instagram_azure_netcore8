using System;
using Domain.EventMessages;

namespace PostService.Services
{
	public interface IUnlikeCreatedProcessorService
	{
        Task UpdateLike(UnlikeCreated unlikeCreated);

    }
}

