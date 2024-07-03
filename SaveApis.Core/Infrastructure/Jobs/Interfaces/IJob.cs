using SaveApis.Core.Infrastructure.Events.Interfaces;

namespace SaveApis.Core.Infrastructure.Jobs.Interfaces;

public interface IJob<in TEvent> where TEvent : IEvent
{
    Task RunAsync(TEvent @event, CancellationToken cancellationToken = default);
}