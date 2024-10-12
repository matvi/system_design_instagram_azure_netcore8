using System;
using System.Text.Json;
using Domain.EventMessages;
using Domain.Strategies;
using PostService.Services;

namespace PostService.Strategies
{
	public class UnlikeCreatedEventProcessorStrategy : IEventProcessorStrategy
    {
        private readonly IUnlikeCreatedProcessorService _unlikeCreatedProcessorService;

        public UnlikeCreatedEventProcessorStrategy(IUnlikeCreatedProcessorService unlikeCreatedProcessorService)
		{
            _unlikeCreatedProcessorService = unlikeCreatedProcessorService;
        }

        public async Task ProcessEvent(string eventMessage)
        {
            var unlikeEvent = JsonSerializer.Deserialize<UnlikeCreated>(eventMessage);
            if(unlikeEvent is null)
            {
                return;
            }

            await _unlikeCreatedProcessorService.UpdateLike(unlikeEvent);
        }
    }
}

