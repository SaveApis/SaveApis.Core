using Hangfire;
using MediatR;
using SaveApis.Core.Application.Events;
using SaveApis.Core.Infrastructure.Events.Interfaces;
using SaveApis.Core.Infrastructure.Jobs;
using Serilog;
using Serilog.Events;

namespace SaveApis.Core.Application.Jobs.Hangfire;

[Queue("system")]
public class RegisterRecurringEventsJob(ILogger logger, IRecurringJobManager manager, IMediator mediator, IEnumerable<IRecurringEvent> events) : BaseJob<ApplicationStartedEvent>(logger)
{
    [JobDisplayName("Register or update recurring events")]
    public override Task RunAsync(ApplicationStartedEvent @event, CancellationToken cancellationToken = default)
    {
        foreach (var jobEvent in events)
        {
            Log(LogEventLevel.Information, "Register {Name}", null, jobEvent.GetType().Name);
            manager.AddOrUpdate(jobEvent.Id, () => Publish(jobEvent, cancellationToken), jobEvent.CronExpression);
        }
        return Task.CompletedTask;
    }

    [JobDisplayName("Event: {0}")]
    public async Task Publish(IEvent jobEvent, CancellationToken cancellationToken)
    {
        await mediator.Publish(jobEvent, cancellationToken);
    }
}