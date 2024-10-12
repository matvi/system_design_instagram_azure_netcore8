using System;
namespace Common.ServiceBus
{
	public interface IHandleServiceBusMessage
	{
        Task HandleStringMessageAsync(string message);
    }
}

