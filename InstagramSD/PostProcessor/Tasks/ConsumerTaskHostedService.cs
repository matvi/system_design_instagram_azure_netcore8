using Common.Tasks;
using Microsoft.Extensions.Options;
using PostProcessor.Settings;
using PostProcessor.TaskServices;

namespace PostProcessor.Tasks
{
    public class ConsumerTaskHostedService : CronJobServiceBase
    {
        private readonly ConsumerTaskServiceSettings _hostedServiceSettings;
        private readonly ILogger<ConsumerTaskHostedService> _log;
        private readonly IServiceProvider _serviceProvider;
        private AsyncServiceScope _scope; 

        public ConsumerTaskHostedService(
            IOptions<ConsumerTaskServiceSettings> hostedServiceSettings,
            ILogger<ConsumerTaskHostedService> log,
            IServiceProvider serviceProvider) : base(hostedServiceSettings, log)
        {
            _hostedServiceSettings = hostedServiceSettings.Value;
            _log = log;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteTaskAsync(CancellationToken cancellationToken)
        {
            _scope = _serviceProvider.CreateAsyncScope();
            var taskService = _scope.ServiceProvider.GetRequiredService<IPostTopicConsumerTaskService>();
            await taskService.StartAsync(cancellationToken);
        }

        protected override async Task DisposeScope()
        {
            await _scope.DisposeAsync();
        }
    }
}

