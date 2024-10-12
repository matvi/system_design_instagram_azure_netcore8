using System;
using Common.ServiceBus;
using Microsoft.Extensions.Options;
using RankService.Settings;

namespace RankService.Services.ServiceBusConsumers
{
	public class LikeTopicServiceBusConsumer : ServiceBusConsumerBase
    {
		public LikeTopicServiceBusConsumer(IOptions<LikeTopicServiceBusConsumerSettings> likeTopicServiceBusConsumerSettings)
			:base(likeTopicServiceBusConsumerSettings)
		{
		}
	}
}

