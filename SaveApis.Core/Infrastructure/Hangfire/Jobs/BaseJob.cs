using SaveApis.Core.Infrastructure.Hangfire.Events;
using Serilog;

namespace SaveApis.Core.Infrastructure.Hangfire.Jobs;

public abstract class BaseJob<TEvent> : IJob<TEvent> where TEvent : IEvent
{
    protected BaseJob(ILogger logger)
    {
        Logger = logger.ForContext("job.type", GetType().FullName).ForContext("job.name", GetType().Name);
    }

    protected ILogger Logger { get; }

    public abstract Task<bool> CanExecute(TEvent @event);
    public abstract Task RunAsync(TEvent @event, CancellationToken cancellationToken);
}
