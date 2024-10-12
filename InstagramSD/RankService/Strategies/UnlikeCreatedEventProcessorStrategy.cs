using System;
using Domain.Strategies;

namespace RankService.Strategies
{
	public class UnlikeCreatedEventProcessorStrategy : IEventProcessorStrategy
    {
		public UnlikeCreatedEventProcessorStrategy()
		{
		}

        public Task ProcessEvent(string eventMessage)
        {
            Console.WriteLine($"Processing unlikeCreated event " + eventMessage);
            return Task.CompletedTask;
        }
    }
}

