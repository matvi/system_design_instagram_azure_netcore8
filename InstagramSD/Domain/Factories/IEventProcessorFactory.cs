using System;
using Domain.Enums;
using Domain.Strategies;

namespace Domain.Factories
{
	public interface IEventProcessorFactory
	{
        IEventProcessorStrategy GetEventProcessorStrategy(EventType eventType);
    }
}

