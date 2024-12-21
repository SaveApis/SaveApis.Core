using SaveApis.Core.Infrastructure.Hangfire.Events;

namespace SaveApis.Core.Infrastructure.Hangfire.Jobs;

public interface IJob<in TEvent> where TEvent : IEvent
{
    bool CanExecute(TEvent @event);
    Task RunAsync(TEvent @event, CancellationToken cancellationToken);
}
