using SaveApis.Core.Application.Hangfire;
using SaveApis.Core.Infrastructure.Hangfire.Attributes;
using SaveApis.Core.Infrastructure.Hangfire.Jobs;
using SaveApis.Example.Domains.Hangfire.Application.Hangfire.Events.Recurring;
using ILogger = Serilog.ILogger;

namespace SaveApis.Example.Domains.Hangfire.Application.Hangfire.Jobs;

[HangfireQueue(HangfireQueue.Low)]
public class TestRecurringJob(ILogger logger) : BaseJob<TestRecurringEvent>(logger)
{
    public override Task<bool> CanExecute(TestRecurringEvent @event)
    {
        return Task.FromResult(true);
    }

    [HangfireJobName("Test recurring job")]
    public override Task RunAsync(TestRecurringEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
