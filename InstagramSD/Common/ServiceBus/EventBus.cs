using System;
using Azure.Messaging.ServiceBus;
using Common.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Common.ServiceBus
{
    public class EventBus : IEventBus
    {
        private readonly ServiceBusSettings _serviceBusSettings;

        public EventBus(IOptions<ServiceBusSettings> serviceBusSettings)
        {
            _serviceBusSettings = serviceBusSettings.Value;
        }

        public async Task<bool> PublishMessage<T>(T messageRequest)
        {
            var data = JsonSerializer.Serialize(messageRequest);
            var client = new ServiceBusClient(_serviceBusSettings.ConnectionString);
            var sender = client.CreateSender(_serviceBusSettings.Topic);
            using ServiceBusMessageBatch serviceBusClientMessageBatch = await sender.CreateMessageBatchAsync();
            serviceBusClientMessageBatch.TryAddMessage(new ServiceBusMessage(data));
            await sender.SendMessagesAsync(serviceBusClientMessageBatch);
            await sender.DisposeAsync();
            await client.DisposeAsync();
            return true;
        }
    }
}

