using System;
using System.Text.Json;
using Domain.EventMessages;
using Domain.Strategies;
using PostService.Services;

namespace PostService.Strategies
{
	public class LikeCreatedEventProcessorStrategy : IEventProcessorStrategy
    {
        private readonly ILikeCreatedProcessorService _likeCreatedProcessorService;

        public LikeCreatedEventProcessorStrategy(ILikeCreatedProcessorService likeCreatedProcessorService)
		{
            _likeCreatedProcessorService = likeCreatedProcessorService;
        }

        public async Task ProcessEvent(string eventMessage)
        {
            Console.WriteLine("Processing event from internal PostEventProcessorStrategy");
            var likeCreated = JsonSerializer.Deserialize<LikeCreated>(eventMessage);
            if(likeCreated is null)
            {
                return;
            }
            await _likeCreatedProcessorService.ProcessLikeEvent(likeCreated);
        }
    }
}

