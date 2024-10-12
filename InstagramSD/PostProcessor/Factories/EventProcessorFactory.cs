using System;
using Common.Strategies.EventProcessors;
using Domain.Enums;
using Domain.Factories;
using Domain.Strategies;
using PostProcessor.Strategies;

namespace PostProcessor.Factories
{
	public class EventProcessorFactory : IEventProcessorFactory
	{
        private readonly IServiceProvider _serviceProvider;

        public EventProcessorFactory(IServiceProvider serviceProvider)
		{
            _serviceProvider = serviceProvider;
        }

        public IEventProcessorStrategy GetEventProcessorStrategy(EventType eventType)
        {
            var services = _serviceProvider.GetServices<IEventProcessorStrategy>();
            return eventType switch
            {
                EventType.PostCreated => services.FirstOrDefault(o => o.GetType() == typeof(PostCreatedEventProcessorStrategy)),
                _ => null
            };
        }
    }
}

