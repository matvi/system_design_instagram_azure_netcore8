using System;
namespace Common.TaskServices
{
	public interface ITaskService
	{
        public Task StartAsync(CancellationToken cancellationToken);
    }
}

