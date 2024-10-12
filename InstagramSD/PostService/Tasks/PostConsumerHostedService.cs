using System;
using Common.Settings;
using Common.Tasks;
using Microsoft.Extensions.Options;
using PostService.Settings;
using PostService.TaskServices;

namespace PostService.Tasks
{
    public class PostConsumerHostedService : CronJobServiceBase
    {
        private readonly IServiceProvider _service;
        private AsyncServiceScope _scope;

        public PostConsumerHostedService(
            IOptions<PostConsumerHostedServiceSettings> postConsumerHostedServiceSettings,
            ILogger<CronJobServiceBase> log,
            IServiceProvider service)
            : base(postConsumerHostedServiceSettings, log)
        {
            _service = service;
        }

        protected override async Task ExecuteTaskAsync(CancellationToken cancellationToken)
        {
            _scope = _service.CreateAsyncScope();
            var postTaskService = _scope.ServiceProvider.GetRequiredService<ILikeTopicConsumerTaskService>();
            await postTaskService.StartAsync(cancellationToken);
        }

        protected override async Task DisposeScope()
        {
            await _scope.DisposeAsync();
        }
    }
}

