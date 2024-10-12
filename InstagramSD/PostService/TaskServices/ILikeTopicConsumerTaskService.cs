using System;
using Common.ServiceBus;
using Common.TaskServices;

namespace PostService.TaskServices
{
	public interface ILikeTopicConsumerTaskService : ITaskService, IHandleServiceBusMessage
    {
	}
}

