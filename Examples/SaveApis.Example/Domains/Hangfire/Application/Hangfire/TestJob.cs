using SaveApis.Core.Application.Hangfire.Events;
using SaveApis.Core.Infrastructure.Hangfire.Attributes;
using SaveApis.Core.Infrastructure.Hangfire.Jobs;
using ILogger = Serilog.ILogger;

namespace SaveApis.Example.Domains.Hangfire.Application.Hangfire;

public class TestJob(ILogger logger) : BaseJob<ApplicationStartedEvent>(logger)
{
    public override bool CanExecute(ApplicationStartedEvent @event)
    {
        return true;
    }

    [HangfireJobName("Test job")]
    public override Task RunAsync(ApplicationStartedEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
