using System;
namespace Common.Settings
{
	public abstract class ServiceBusConsumerSettingsBase
	{
        public string ConnectionString { get; set; }
        public string Topic { get; set; }
        public string Subscription { get; set; }
    }
}

