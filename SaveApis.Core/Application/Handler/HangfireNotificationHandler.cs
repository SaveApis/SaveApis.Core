using Hangfire;
using MediatR;
using SaveApis.Core.Infrastructure.Events.Interfaces;
using SaveApis.Core.Infrastructure.Jobs.Interfaces;
using Serilog;

namespace SaveApis.Core.Application.Handler;

public class HangfireNotificationHandler<TNotification>(ILogger logger, IBackgroundJobClient backgroundJobClient, IEnumerable<IJob<TNotification>> assignedJobs)
    : INotificationHandler<TNotification> where TNotification : IEvent
{
    public Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        logger.Information("Received Hangfire notification: {Notification}", notification.GetType().Name);
        foreach (var job in assignedJobs)
        {
            logger.Debug("Enqueue job: {Name}", job.GetType().Name);
            backgroundJobClient.Enqueue(() => job.RunAsync(notification, cancellationToken));
        }

        return Task.CompletedTask;
    }
}