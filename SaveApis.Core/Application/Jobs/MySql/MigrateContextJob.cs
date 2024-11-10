using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Application.Events;
using SaveApis.Core.Application.Events.MySql;
using SaveApis.Core.Infrastructure.Jobs;
using SaveApis.Core.Infrastructure.Persistence.MySql.Interfaces;
using Serilog;
using Serilog.Events;

namespace SaveApis.Core.Application.Jobs.MySql;

[Queue("system")]
public class MigrateContextJob(ILogger logger, IMediator mediator, IDbContextFactory factory)
    : BaseModuleSpecifiedJob<ApplicationStartedEvent>(logger)
{
    [JobDisplayName("Migrate DbContext")]
    public override async Task RunAsync(ApplicationStartedEvent @event, CancellationToken cancellationToken = default)
    {
        var dbContexts = factory.CreateAll();
        Log(LogEventLevel.Information, "Found {Count} Context", null, dbContexts.Count);

        foreach (var context in dbContexts)
        {
            Log(LogEventLevel.Information, "Migrating Context: {Name}", null, context.GetType().Name);
            try
            {
                await context.Database.MigrateAsync(cancellationToken);
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