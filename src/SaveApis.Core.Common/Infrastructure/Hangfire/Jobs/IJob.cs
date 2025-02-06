using Hangfire.Server;
using SaveApis.Core.Common.Infrastructure.Hangfire.Events;

namespace SaveApis.Core.Common.Infrastructure.Hangfire.Jobs;

/// <summary>
/// Interface to define a job that can be executed by <see cref="TEvent"/>
/// </summary>
/// <typeparam name="TEvent">Event which triggers the job</typeparam>
public interface IJob<in TEvent> where TEvent : IEvent
{
    /// <summary>
    /// Method to check if the job supports the event
    /// </summary>
    /// <param name="event">Instance of the event</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>True, if the job supports the event</returns>
    Task<bool> CheckSupportAsync(TEvent @event, CancellationToken cancellationToken = default);

    /// <summary>
    /// Method to run the job
    /// </summary>
    /// <param name="event">Instance of the event</param>
    /// <param name="performContext">Perform context of the Job. Is getting injected by Hangfire</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    Task RunAsync(TEvent @event, PerformContext? performContext = null, CancellationToken cancellationToken = default);
}
