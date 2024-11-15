using Hangfire;
using MediatR;
using SaveApis.Core.Infrastructure.Events.Interfaces;
using SaveApis.Core.Infrastructure.Jobs.Interfaces;
using Serilog;

namespace SaveApis.Core.Application.Handler;

public class HangfireNotificationHandler<TNotification>(
    ILogger logger,
    IBackgroundJobClient backgroundJobClient,
    IEnumerable<IJob<TNotification>> assignedJobs)
    : INotificationHandler<TNotification> where TNotification : IEvent
{
    public Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        var scopedLogger = logger.ForContext("event", notification.GetType().FullName);
        scopedLogger.Information("Received Hangfire notification");

        foreach (var job in assignedJobs)
        {
            scopedLogger.Information("Enqueue job: {Name}", job.GetType().FullName);
            backgroundJobClient.Enqueue(() => job.RunAsync(notification, cancellationToken));
        }

        return Task.CompletedTask;
    }
}