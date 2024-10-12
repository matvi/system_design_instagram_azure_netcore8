using System;
using Common.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RankService.Settings;
using RankService.TaskServices;

namespace RankService.Tasks
{
	public class LikeConsumerHostedService : CronJobServiceBase
    {
        private AsyncServiceScope _scope;
        private readonly IServiceProvider _serviceProvider;

        public LikeConsumerHostedService(
			IOptions<LikeConsumerHostedServiceSettings> likeConsumerHostedServiceSettings,
			ILogger<LikeConsumerHostedService> logger,
            IServiceProvider serviceProvider) : base(likeConsumerHostedServiceSettings, logger)
		{
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteTaskAsync(CancellationToken cancellationToken)
        {
            _scope = _serviceProvider.CreateAsyncScope();
            var service = _scope.ServiceProvider.GetService<ILikeTopicConsumerTaskService>();
            await service.StartAsync(cancellationToken);
        }

        protected override async Task DisposeScope()
        {
            await _scope.DisposeAsync();
        }
    }
}

