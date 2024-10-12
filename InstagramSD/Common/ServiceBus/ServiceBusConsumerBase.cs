using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Common.Settings;
using Microsoft.Extensions.Options;

namespace Common.ServiceBus
{
    public abstract class ServiceBusConsumerBase: IServiceBusConsumer
	{
        private readonly ServiceBusConsumerSettingsBase _serviceBusConsummerSettings;

        public ServiceBusConsumerBase(IOptions<ServiceBusConsumerSettingsBase> serviceBusConsummerSettings)
		{
            _serviceBusConsummerSettings = serviceBusConsummerSettings.Value;
		}

        //todo: pass IHandleServiceBusMessage as Function instead of a class
        public async Task StartAsync(IHandleServiceBusMessage handleServiceBusMessage,CancellationToken cancellationToken)
        {
            var serviceBusClient = new ServiceBusClient(_serviceBusConsummerSettings.ConnectionString);
            var processor = serviceBusClient.CreateProcessor(_serviceBusConsummerSettings.Topic, _serviceBusConsummerSettings.Subscription, new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 1
            });


            // handle received messages
            processor.ProcessMessageAsync += args => MessageStringHandler(args, handleServiceBusMessage);
            processor.ProcessErrorAsync += ErrorHandler;

            // Start processing
            await processor.StartProcessingAsync(cancellationToken);

            Console.WriteLine("Task running");

        }

        // handle received messages
        async Task MessageStringHandler(ProcessMessageEventArgs args, IHandleServiceBusMessage handleServiceBusMessage)
        {
            var body = args.Message.Body.ToString();
            Console.WriteLine($"Received: imageFileName {body}");

            await handleServiceBusMessage.HandleStringMessageAsync(body);

            // complete the message. messages are deleted from the subscription. 
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

    }
}

