using Hangfire;
using Hangfire.Storage;
using SaveApis.Core.Application.Events;
using SaveApis.Core.Infrastructure.Events.Interfaces;
using SaveApis.Core.Infrastructure.Jobs;
using Serilog;
using Serilog.Events;

namespace SaveApis.Core.Application.Jobs.Hangfire;


[Queue("system")]
public class CleanupRecurringEventsJob(ILogger logger, IRecurringJobManager manager, IEnumerable<IRecurringEvent> events) : BaseJob<ApplicationStartedEvent>(logger)
{
    [JobDisplayName("Remove outdated recurring events")]
    public override Task RunAsync(ApplicationStartedEvent @event, CancellationToken cancellationToken = default)
    {
        var recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs() ?? [];
        var idsToRemove = recurringJobs.Where(it => events.All(e => it.Id != e.Id)).Select(it => it.Id).ToList();

        foreach (var id in idsToRemove)
        {
            Log(LogEventLevel.Information, "Remove outdated Event: {Name}", null, id);
            manager.RemoveIfExists(id);
        }

        return Task.CompletedTask;
    }
}