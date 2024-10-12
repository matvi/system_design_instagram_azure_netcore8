using System;
using Domain.Enums;
using Domain.Factories;
using Domain.Strategies;
using RankService.Strategies;

namespace RankService.Factories
{
	public class RankEventProcessorFactory :IEventProcessorFactory
	{
        private readonly IServiceProvider _serviceProvider;

        public RankEventProcessorFactory(IServiceProvider serviceProvider)
		{
            _serviceProvider = serviceProvider;
        }

        public IEventProcessorStrategy GetEventProcessorStrategy(EventType eventType)
        {
            var services = _serviceProvider.GetServices<IEventProcessorStrategy>();

            return eventType switch {
                EventType.FollowCreated => services.FirstOrDefault(o => o.GetType() == typeof(FollowCreatedEventProccessorStrategy)),
                EventType.UnfollowCreated => services.FirstOrDefault(o => o.GetType() == typeof(UnfollowCreatedEventProccessorStrategy)),
                EventType.UnlikeCreated => services.FirstOrDefault(o => o.GetType() == typeof(UnlikeCreatedEventProcessorStrategy)),
                EventType.LikeCreated => services.FirstOrDefault(o => o.GetType() == typeof(LikeCreatedEventProcessorStrategy)),
                _ => null
            }; 
        }
    }
}

