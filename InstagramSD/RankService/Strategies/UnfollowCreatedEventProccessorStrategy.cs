using System;
using Domain.Strategies;

namespace RankService.Strategies
{
	public class UnfollowCreatedEventProccessorStrategy : IEventProcessorStrategy
    {
		public UnfollowCreatedEventProccessorStrategy()
		{
		}

        public Task ProcessEvent(string eventMessage)
        {
            Console.WriteLine($"Processing unfollowcreated event " + eventMessage);
            return Task.CompletedTask;
        }
    }
}

