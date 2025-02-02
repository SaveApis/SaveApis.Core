using System.Reflection;
using Autofac;
using Hangfire;
using MediatR;
using SaveApis.Core.Common.Application.Hangfire.Events;
using SaveApis.Core.Common.Application.Hangfire.Types;
using SaveApis.Core.Common.Infrastructure.Hangfire.Attributes;
using SaveApis.Core.Common.Infrastructure.Hangfire.Events;
using SaveApis.Core.Common.Infrastructure.Hangfire.Jobs;
using SaveApis.Core.Common.Infrastructure.Helpers;
using Serilog;

namespace SaveApis.Core.Common.Application.Hangfire.Jobs;

[HangfireQueue(HangfireQueue.System)]
public class RegisterRecurringEventsJob(ILogger logger, IMediator mediator, IRecurringJobManagerV2 manager, IAssemblyHelper assemblyHelper) : BaseJob<ApplicationStartedEvent>(logger)
{
    protected override bool CheckSupport(ApplicationStartedEvent @event)
    {
        return @event.ServerType == HangfireServerType.Server;
    }

    [HangfireJobName("Register recurring events")]
    public override Task RunAsync(ApplicationStartedEvent @event, CancellationToken cancellationToken = default)
    {
        var options = new RecurringJobOptions
        {
            TimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"),
        };

        foreach (var type in assemblyHelper.GetTypesByAttribute<HangfireRecurringEventAttribute>().Where(it => it.IsAssignableTo<IEvent>()))
        {
            var attribute = type.GetCustomAttribute<HangfireRecurringEventAttribute>()!;
            var id = attribute.Id;
            var cron = attribute.Cron;
            var queue = attribute.Queue;

            manager.AddOrUpdate(id, queue.ToString().ToLowerInvariant(), () => PublishEventAsync((IEvent)Activator.CreateInstance(type)!, cancellationToken), () => cron, options);
        }

        return Task.CompletedTask;
    }

    [HangfireJobName("Event: {0}")]
    public async Task PublishEventAsync(IEvent @event, CancellationToken cancellationToken = default)
    {
        Logger.Information("Recurring Event: {Name}", @event.GetType().FullName);
        await mediator.Publish(@event, cancellationToken).ConfigureAwait(false);
    }
}
