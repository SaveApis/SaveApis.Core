using Example.Web.Domains.Hangfire.Application.Hangfire.Events;
using SaveApis.Core.Common.Application.Hangfire.Types;
using SaveApis.Core.Common.Infrastructure.Hangfire.Attributes;
using SaveApis.Core.Common.Infrastructure.Hangfire.Jobs;
using ILogger = Serilog.ILogger;

namespace Example.Web.Domains.Hangfire.Application.Hangfire.Jobs;

[HangfireQueue(HangfireQueue.High)]
public class TestJob(ILogger logger) : BaseJob<TestRecurringEvent>(logger)
{
    [HangfireJobName("Test Job")]
    public override Task RunAsync(TestRecurringEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
