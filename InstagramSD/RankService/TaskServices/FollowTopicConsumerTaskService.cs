using System;
using Domain.Enums;
using System.Text.Json;
using Domain.Factories;
using Common.ServiceBus;

namespace RankService.TaskServices
{
	public class FollowTopicConsumerTaskService : IFollowTopicConsumerTaskService
	{
        private readonly IEventProcessorFactory _eventProcessorFactory;
        private readonly IServiceBusConsumer _serviceBusConsumer;

        public FollowTopicConsumerTaskService(
            IEventProcessorFactory eventProcessorFactory,
            [FromKeyedServices("FollowTopicServiceBusConsumer")] IServiceBusConsumer serviceBusConsumer)
		{
            _eventProcessorFactory = eventProcessorFactory;
            _serviceBusConsumer = serviceBusConsumer;
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

