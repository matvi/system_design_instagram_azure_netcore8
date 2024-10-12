using System;
namespace Domain.Strategies
{
	public interface IEventProcessorStrategy
	{
		Task ProcessEvent(string eventMessage);
	}
}