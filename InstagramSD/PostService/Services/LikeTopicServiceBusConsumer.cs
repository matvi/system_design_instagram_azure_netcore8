using System;
using Common.ServiceBus;
using Microsoft.Extensions.Options;
using PostService.Settings;

namespace PostService.Services
{
	public class LikeTopicServiceBusConsumer : ServiceBusConsumerBase
    {
		public LikeTopicServiceBusConsumer(IOptions<LikeTopicServiceBusConsumerSettings> likeTopicServiceBusConsumerSettings):base(likeTopicServiceBusConsumerSettings)
		{
		}
	}
}

