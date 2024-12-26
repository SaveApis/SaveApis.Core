using System.Reflection;
using Hangfire;
using MediatR;
using SaveApis.Core.Infrastructure.Hangfire.Attributes;
using SaveApis.Core.Infrastructure.Hangfire.Events;
using SaveApis.Core.Infrastructure.Hangfire.Jobs;
using Serilog;

namespace SaveApis.Core.Application.Hangfire.Handlers;

public class HangfireEventHandler<TEvent>(ILogger logger, IBackgroundJobClient client, IEnumerable<IJob<TEvent>> jobs)
    : INotificationHandler<TEvent> where TEvent : IEvent
{
    public async Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        var jobList = jobs.ToList();
        logger.Information("Receiving event {Event} with {Count} jobs", notification.GetType().FullName, jobList.Count);

        foreach (var job in jobList)
        {
            var canExecute = await job.CanExecute(notification).ConfigureAwait(false);
            if (!canExecute)
            {
                logger.Debug("Skipping job {Job} for event {Event}", job.GetType().FullName, notification.GetType().FullName);
                continue;
            }

            var queue = ReadJobQueue(job);
            logger.Debug("Executing job {Job} ({Queue}) for event {Event}", job.GetType().FullName, queue, notification.GetType().FullName);
            client.Enqueue(queue.ToString().ToLowerInvariant(), () => job.RunAsync(notification, CancellationToken.None));
        }
    }

    private HangfireQueue ReadJobQueue(IJob<TEvent> job)
    {
        var attribute = job.GetType().GetCustomAttribute<HangfireQueueAttribute>();
        if (attribute is not null)
        {
            return attribute.Queue;
        }

        logger.Warning("Job {Job} does not have a {Name}! Using default queue!", job.GetType().FullName, nameof(HangfireQueueAttribute));

        return HangfireQueue.Medium;
    }
}
