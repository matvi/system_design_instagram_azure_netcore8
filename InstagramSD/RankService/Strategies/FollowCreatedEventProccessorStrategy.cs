using System;
using Domain.Strategies;

namespace RankService.Strategies
{
	public class FollowCreatedEventProccessorStrategy : IEventProcessorStrategy
    {
		public FollowCreatedEventProccessorStrategy()
		{
		}

        public Task ProcessEvent(string eventMessage)
        {
            Console.WriteLine($"Processing followcreated event " + eventMessage);
            return Task.CompletedTask;
        }
    }
}

