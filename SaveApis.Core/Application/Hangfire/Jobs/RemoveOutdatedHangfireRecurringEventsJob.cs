﻿using System.Reflection;
using Hangfire;
using Hangfire.Storage;
using MediatR;
using SaveApis.Core.Application.Hangfire.Events;
using SaveApis.Core.Infrastructure.Hangfire.Attributes;
using SaveApis.Core.Infrastructure.Hangfire.Events;
using SaveApis.Core.Infrastructure.Hangfire.Jobs;
using Serilog;

namespace SaveApis.Core.Application.Hangfire.Jobs;

[HangfireQueue(HangfireQueue.System)]
public class RemoveOutdatedHangfireRecurringEventsJob(
    ILogger logger,
    JobStorage storage,
    IRecurringJobManager manager,
    IMediator mediator,
    IEnumerable<IEvent> events) : BaseJob<ApplicationStartedEvent>(logger)
{
    public override bool CanExecute(ApplicationStartedEvent @event)
    {
        return true;
    }

    [HangfireJobName("Remove outdated hangfire recurring events")]
    public override async Task RunAsync(ApplicationStartedEvent @event, CancellationToken cancellationToken)
    {
        var recurringJobs = storage.GetConnection().GetRecurringJobs();
        var jobsWithoutExecution = recurringJobs.Where(x => x.LastExecution is null).ToList();

        foreach (var job in jobsWithoutExecution)
        {
            manager.RemoveIfExists(job.Id);
        }

        recurringJobs.RemoveAll(jobsWithoutExecution.Contains);

        // Check if jobs and event contains the same id's
        var jobIds = recurringJobs.ConvertAll(x => x.Id);
        var recurringEvents = events.Where(it => it.GetType().GetCustomAttribute<HangfireRecurringEventAttribute>() is not null)
            .Select(it => it.GetType().GetCustomAttribute<HangfireRecurringEventAttribute>()!.Id)
            .ToList();

        foreach (var jobId in jobIds.Except(recurringEvents).ToList())
        {
            manager.RemoveIfExists(jobId);
        }

        await mediator.Publish(new OutdatedHangfireRecurringEventsRemovedEvent(), cancellationToken).ConfigureAwait(false);
    }
}