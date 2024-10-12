using System;
using Common.Settings;
using Cronos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Common.Tasks
{
    public abstract class CronJobServiceBase : IHostedService, IDisposable
    {
        private readonly ILogger _log;
        private readonly HostedServiceTaskSettingsBase _hostedServiceTaskSettingsBase;
        private System.Timers.Timer _timer;
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;

        protected CronJobServiceBase(IOptions<HostedServiceTaskSettingsBase> hostedServiceSettings, ILogger<CronJobServiceBase> log)
        {
            _log = log;
            _hostedServiceTaskSettingsBase = hostedServiceSettings?.Value;
            _expression = CronExpression.Parse(_hostedServiceTaskSettingsBase.CronExpressionTimer, CronFormat.Standard);
            _timeZoneInfo = TimeZoneInfo.Local;
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation($"{GetType()} is Starting");
            if (_hostedServiceTaskSettingsBase.Active)
            {
                //await ScheduleJob(cancellationToken); // dont need to schedule, only execute once
                await ExecuteTaskAsync(cancellationToken);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation($"{GetType()} is Stopping");
            await DisposeScope();
        }

        private async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                if (delay.TotalMilliseconds <= 0)   // prevent non-positive values from being passed into Timer
                {
                    await ScheduleJob(cancellationToken);
                }
                _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                _timer.Elapsed += async (sender, args) =>
                {
                    _timer.Dispose();  // reset and dispose timer
                    _timer = null;

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await ExecuteTaskAsync(cancellationToken);
                    }

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await ScheduleJob(cancellationToken);    // reschedule next
                    }
                };
                _timer.Start();
            }
            await Task.CompletedTask;
        }

        protected virtual async Task ExecuteTaskAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(5000, cancellationToken);
        }

        protected virtual Task DisposeScope()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispose)
        {
            try
            {
                if (dispose)
                {
                    _timer?.Dispose();
                }
            }
            finally
            {

            }
        }
    }
}

