using System;
using Common.ServiceBus;
using Common.TaskServices;

namespace PostProcessor.TaskServices
{
	public interface IPostTopicConsumerTaskService : ITaskService, IHandleServiceBusMessage
    {
	}
}

