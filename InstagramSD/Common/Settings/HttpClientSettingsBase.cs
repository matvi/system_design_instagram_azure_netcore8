using System;
namespace Common.Settings
{
	public abstract class HttpClientSettingsBase
	{
        public string Name { get; set; }
        public string BaseUrl { get; set; }
    }
}

