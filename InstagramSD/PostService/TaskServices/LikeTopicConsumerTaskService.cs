using System;
using System.Text.Json;
using Common.ServiceBus;
using Domain.Enums;
using Domain.EventMessages;
using Domain.Factories;
using PostService.Services;

namespace PostService.TaskServices
{
	public class LikeTopicConsumerTaskService : ILikeTopicConsumerTaskService
	{
        private readonly IServiceBusConsumer _serviceBusConsumer;
        private readonly ILikeCreatedProcessorService _likeCreatedProcessorService;
        private readonly IEventProcessorFactory _eventProcessorFactory;
        private readonly IServiceProvider _serviceProvider;

        public LikeTopicConsumerTaskService(
            [FromKeyedServices("LikeTopicServiceBusConsumer")] IServiceBusConsumer serviceBusConsumer,
            ILikeCreatedProcessorService likeCreatedProcessorService,
            IEventProcessorFactory eventProcessorFactory,
            IServiceProvider serviceProvider)
		{
            _serviceBusConsumer = serviceBusConsumer;
            _likeCreatedProcessorService = likeCreatedProcessorService;
            _eventProcessorFactory = eventProcessorFactory;
            _serviceProvider = serviceProvider;
        }

        public async Task HandleStringMessageAsync(string message)
        {
            var jsonDocument = JsonDocument.Parse(message);

            // Extract the value of the "type" field
            if (jsonDocument.RootElement.TryGetProperty("EventType", out JsonElement typeElement))
            {
                string type = typeElement.GetString();
                Console.WriteLine("Type: " + type);
                Enum.TryParse(type, out EventType myEvent);

                var eventStrategy = _eventProcessorFactory.GetEventProcessorStrategy(myEvent);
                await eventStrategy.ProcessEvent(message);
            }
            else
            {
                Console.WriteLine("Type field not found in JSON.");
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _serviceBusConsumer.StartAsync(this, cancellationToken);
        }
    }
}

