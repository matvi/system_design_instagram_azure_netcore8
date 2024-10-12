using System;
using Common.ServiceBus;
using Microsoft.Extensions.Options;
using RankService.Settings;

namespace RankService.Services.ServiceBusConsumers
{
	public class FollowTopicServiceBusConsumer : ServiceBusConsumerBase
	{
		public FollowTopicServiceBusConsumer(IOptions<FollowTopicServiceBusConsumerSettings> followTopicServiceBusConsumerSettings)
			:base(followTopicServiceBusConsumerSettings)
		{
		}
	}
}

