using System.Reflection;
using Hangfire;
using MediatR;
using SaveApis.Core.Application.Hangfire.Events;
using SaveApis.Core.Infrastructure.Hangfire.Attributes;
using SaveApis.Core.Infrastructure.Hangfire.Events;
using SaveApis.Core.Infrastructure.Hangfire.Jobs;
using Serilog;

namespace SaveApis.Core.Application.Hangfire.Jobs;

[HangfireQueue(HangfireQueue.System)]
public class RegisterHangfireRecurringEventsJob(ILogger logger, IRecurringJobManager manager, IMediator mediator, IEnumerable<IEvent> events)
    : BaseJob<OutdatedHangfireRecurringEventsRemovedEvent>(logger)
{
    public override bool CanExecute(OutdatedHangfireRecurringEventsRemovedEvent @event)
    {
        return true;
    }

    [HangfireJobName("Register hangfire recurring events")]
    public override Task RunAsync(OutdatedHangfireRecurringEventsRemovedEvent @event, CancellationToken cancellationToken)
    {
        var options = new RecurringJobOptions
        {
            TimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"),
        };

        foreach (var recurringEvent in events)
        {
            var attribute = recurringEvent.GetType().GetCustomAttribute<HangfireRecurringEventAttribute>();
            if (attribute is null)
            {
                continue;
            }

            manager.AddOrUpdate(attribute.Id, attribute.Queue.ToString().ToLower(),
                () => PublishEvent(recurringEvent, CancellationToken.None), () => attribute.CronExpression, options);
        }

        return Task.CompletedTask;
    }

    [HangfireJobName("Publish: {0}")]
    public async Task PublishEvent(IEvent @event, CancellationToken token)
    {
        await mediator.Publish(@event, token).ConfigureAwait(false);
    }
}
