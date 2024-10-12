using System;
using Common.ServiceBus;
using Microsoft.Extensions.Options;
using PostProcessor.Settings;

namespace PostProcessor.Services
{
	public class PostProcessorServiceBusConsumer : ServiceBusConsumerBase
	{
		public PostProcessorServiceBusConsumer(IOptions<PostProcessorServiceBusConsumerSettings> postProcessorServiceBusConsumerSettings)
			:base(postProcessorServiceBusConsumerSettings)
		{
		}
	}
}

