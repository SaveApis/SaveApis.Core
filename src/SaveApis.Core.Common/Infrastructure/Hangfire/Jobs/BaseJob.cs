using Hangfire.Server;
using SaveApis.Core.Common.Infrastructure.Hangfire.Events;
using Serilog;

namespace SaveApis.Core.Common.Infrastructure.Hangfire.Jobs;

public abstract class BaseJob<TEvent>(ILogger logger) : IJob<TEvent> where TEvent : IEvent
{
    protected ILogger Logger { get; } = logger;

    protected virtual bool CheckSupport(TEvent @event)
    {
        return true;
    }

    public virtual Task<bool> CheckSupportAsync(TEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(CheckSupport(@event));
    }

    public abstract Task RunAsync(TEvent @event, PerformContext? performContext = null, CancellationToken cancellationToken = default);
}
