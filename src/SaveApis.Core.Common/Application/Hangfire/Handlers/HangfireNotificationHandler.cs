using System.Reflection;
using Hangfire;
using MediatR;
using SaveApis.Core.Common.Application.Hangfire.Types;
using SaveApis.Core.Common.Infrastructure.Hangfire.Attributes;
using SaveApis.Core.Common.Infrastructure.Hangfire.Events;
using SaveApis.Core.Common.Infrastructure.Hangfire.Jobs;
using Serilog;

namespace SaveApis.Core.Common.Application.Hangfire.Handlers;

public class HangfireNotificationHandler<TEvent>(ILogger logger, IBackgroundJobClientV2 client, IEnumerable<IJob<TEvent>> assignedJobs) : INotificationHandler<TEvent> where TEvent : IEvent
{
    public async Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        var jobs = assignedJobs.ToList();
        logger.Information("Received Event '{Event}' (Jobs: {Count})", notification, jobs.Count);

        foreach (var job in jobs)
        {
            if (!await job.CheckSupportAsync(notification, cancellationToken).ConfigureAwait(false))
            {
                logger.Information("Job '{Job}' does not support Event '{Event}'", job, notification);
                continue;
            }

            var queue = ReadHangfireQueue(job);

            logger.Information("Enqueueing Job '{Job}' to Queue '{Queue}'", job, queue);
            client.Enqueue(queue.ToString().ToLowerInvariant(), () => job.RunAsync(notification, cancellationToken));
        }
    }
    private HangfireQueue ReadHangfireQueue(IJob<TEvent> job)
    {
        var attribute = job.GetType().GetCustomAttribute<HangfireQueueAttribute>();
        if (attribute is null)
        {
            logger.Warning("Job '{Job}' does not have a HangfireQueueAttribute! Using default value", job);

            return HangfireQueue.Medium;
        }

        return attribute.Queue;
    }
}
