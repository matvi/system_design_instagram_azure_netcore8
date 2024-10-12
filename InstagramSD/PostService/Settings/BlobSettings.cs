using System;
namespace PostService.Settings
{
	public class BlobSettings
	{
		public string ConnectionString { set; get; }
		public string ContainerName { get; set; }
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
    }
}

