using System.Text.Json;
using Common.ServiceBus;
using PostProcessor.Services;
using Domain.Enums;
using Domain.Factories;

namespace PostProcessor.TaskServices
{
    public class PostTopicConsumerTaskService : IPostTopicConsumerTaskService
    {
        private readonly IServiceBusConsumer _serviceBusConsumer;
        private readonly IEventProcessorFactory _eventProcessorFactory;

        public PostTopicConsumerTaskService(
            IServiceBusConsumer serviceBusConsumer,
            IEventProcessorFactory eventProcessorFactory)
        {
            _serviceBusConsumer = serviceBusConsumer;
            _eventProcessorFactory = eventProcessorFactory;
        }

        public async Task HandleStringMessageAsync(string message)
        {
            var jsonDocument = JsonDocument.Parse(message);

            if (!jsonDocument.RootElement.TryGetProperty("EventType", out JsonElement typeElement))
            {
                Console.WriteLine("Type field not found in JSON.");
                return;
            }

            string type = typeElement.GetString();
            Console.WriteLine("Type: " + type);
            Enum.TryParse(type, out EventType myEvent);
            var eventStrategy = _eventProcessorFactory.GetEventProcessorStrategy(myEvent);
            await eventStrategy.ProcessEvent(message);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _serviceBusConsumer.StartAsync(this,cancellationToken);
           
        }
    }
}

