using System;
using Domain.Strategies;

namespace RankService.Strategies
{
	public class LikeCreatedEventProcessorStrategy : IEventProcessorStrategy
    {
		public LikeCreatedEventProcessorStrategy()
		{
		}

        public Task ProcessEvent(string eventMessage)
        {
            Console.WriteLine($"Processing likecreated event " + eventMessage);
            return Task.CompletedTask;
        }
    }
}

