using System;
using System.Text.Json;
using Domain.EventMessages;
using Domain.Strategies;
using Microsoft.Azure.Amqp.Framing;
using PostProcessor.Services;

namespace PostProcessor.Strategies
{
	public class PostCreatedEventProcessorStrategy: IEventProcessorStrategy
    {
        private readonly IProcessPostCreatedService _processPostCreatedService;

        public PostCreatedEventProcessorStrategy(IProcessPostCreatedService processPostCreatedService)
        {
            _processPostCreatedService = processPostCreatedService;
        }

        public async Task ProcessEvent(string eventMessage)
        {
            var postCreated = JsonSerializer.Deserialize<PostCreated>(eventMessage);
            if (postCreated is null)
            {
                Console.WriteLine($"PostCreate Message Handler in PostProcessor service could not deserialize event message {eventMessage}");
                return;
            }
            await _processPostCreatedService.Process(postCreated);
            Console.WriteLine("PostCreate Message Handler in PostProcessor service");
        }
    }
}

