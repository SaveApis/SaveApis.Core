using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Application.Events;
using SaveApis.Core.Application.Events.MySql;
using SaveApis.Core.Infrastructure.Jobs;
using Serilog;
using Serilog.Events;

namespace SaveApis.Core.Application.Jobs.MySql;

[Queue("system")]
public class MigrateContextJob(ILogger logger, IMediator mediator, IEnumerable<DbContext> registeredContexts) : BaseJob<ApplicationStartedEvent>(logger)
{
    [JobDisplayName("Migrate DbContext")]
    public override async Task RunAsync(ApplicationStartedEvent @event, CancellationToken cancellationToken = default)
    {
        Log(LogEventLevel.Information, "Found {Count} Context", null, registeredContexts.Count());

        foreach (var context in registeredContexts)
        {
            Log(LogEventLevel.Information, "Migrating Context: {Name}", null, context.GetType().Name);
            try
            {
                await context.Database.MigrateAsync(cancellationToken: cancellationToken);
            }
            catch (Exception e)
            {
                Log(LogEventLevel.Error, "Error migrating Context: {Name}", e, context.GetType().Name);
                throw;
            }
        }

        await mediator.Publish(new MigrationCompletedEvent(), cancellationToken);
    }
}