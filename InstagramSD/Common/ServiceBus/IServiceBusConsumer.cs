namespace Common.ServiceBus
{
    public interface IServiceBusConsumer
    {
        Task StartAsync(IHandleServiceBusMessage handleServiceBusMessage, CancellationToken cancellationToken);
    }
}