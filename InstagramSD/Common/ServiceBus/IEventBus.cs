using System;
namespace Common.ServiceBus
{
    public interface IEventBus
    {
        Task<bool> PublishMessage<T>(T messageRequest);
    }
}

