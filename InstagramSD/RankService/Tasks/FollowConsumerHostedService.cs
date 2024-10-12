using System;
using Common.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RankService.Settings;
using RankService.TaskServices;

namespace RankService.Tasks
{
	public class FollowConsumerHostedService : CronJobServiceBase
	{
        private AsyncServiceScope _scope;
        private IServiceProvider _serviceProvider;

		public FollowConsumerHostedService(
            IOptions<FollowConsumerHostedServiceSettings> followConsumerHostedServiceSettings,
            ILogger<FollowConsumerHostedService> log,
            IServiceProvider serviceProvider)
			:base(followConsumerHostedServiceSettings, log)
		{
            _serviceProvider = serviceProvider;
		}

        protected override async Task ExecuteTaskAsync(CancellationToken cancellationToken)
        {
            _scope = _serviceProvider.CreateAsyncScope();
            var service = _scope.ServiceProvider.GetService<IFollowTopicConsumerTaskService>();
            await service.StartAsync(cancellationToken);
        }

        protected override async Task DisposeScope()
        {
            await _scope.DisposeAsync();
        }
    }
}

