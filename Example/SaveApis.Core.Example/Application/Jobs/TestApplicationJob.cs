using Hangfire;
using SaveApis.Core.Application.Events;
using SaveApis.Core.Infrastructure.Jobs;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace SaveApis.Core.Example.Application.Jobs;

public class TestApplicationJob(ILogger logger) : BaseJob<ApplicationStartedEvent>(logger)
{
    [JobDisplayName("Test Job")]
    public override Task RunAsync(ApplicationStartedEvent @event, CancellationToken cancellationToken = default)
    {
        Log(LogEventLevel.Information, "Job executed at application start! {EventName}", @event.GetType().Name);
        return Task.CompletedTask;
    }
}