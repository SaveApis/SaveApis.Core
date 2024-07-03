using Hangfire;
using SaveApis.Core.Example.Application.Events.Recurring;
using SaveApis.Core.Infrastructure.Jobs;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace SaveApis.Core.Example.Application.Jobs;

public class TestRecurringJob(ILogger logger) : BaseJob<TestRecurringEvent>(logger)
{
    [JobDisplayName("Test recurring job")]
    public override Task RunAsync(TestRecurringEvent @event, CancellationToken cancellationToken = default)
    {
        Log(LogEventLevel.Information, "Test recurring executed!");
        return Task.CompletedTask;
    }
}