using System;
using Common.ServiceBus;
using Common.TaskServices;

namespace RankService.TaskServices
{
	public interface IFollowTopicConsumerTaskService : ITaskService, IHandleServiceBusMessage
	{
	}
}

