using System;
using Domain.EventMessages;
using Domain.interfaces;
using Domain.Strategies;

namespace Common.Strategies.EventProcessors
{
	public abstract class PostEventStrategyBase : IEventProcessorStrategy
	{
		

		public void ProcessEvent(string eventMessage){
		}

        Task IEventProcessorStrategy.ProcessEvent(string eventMessage)
        {
            throw new NotImplementedException();
        }
    }
}

