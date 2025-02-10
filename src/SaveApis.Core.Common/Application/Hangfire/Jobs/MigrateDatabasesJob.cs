using Hangfire.Console;
using Hangfire.Server;
using Hangfire.Throttling;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SaveApis.Core.Common.Application.Hangfire.Events;
using SaveApis.Core.Common.Application.Hangfire.Types;
using SaveApis.Core.Common.Application.Types;
using SaveApis.Core.Common.Infrastructure.Hangfire.Attributes;
using SaveApis.Core.Common.Infrastructure.Hangfire.Jobs;
using SaveApis.Core.Common.Infrastructure.Persistence.Sql;
using Serilog;

namespace SaveApis.Core.Common.Application.Hangfire.Jobs;

[HangfireQueue(HangfireQueue.High)]
[Mutex("core:database:migrate")]
public class MigrateDatabasesJob(ILogger logger, IMediator mediator, IEnumerable<IDesignTimeDbContextFactory<BaseDbContext>> factories) : BaseJob<ApplicationStartedEvent>(logger)
{
    protected override bool CheckSupport(ApplicationStartedEvent @event)
    {
        return @event.ServerType == ServerType.Server;
    }

    [HangfireJobName("{0}: Migrate databases")]
    public override async Task RunAsync(ApplicationStartedEvent @event, PerformContext? performContext = null, CancellationToken cancellationToken = default)
    {
        var bar = performContext.WriteProgressBar("Migrating databases");

        foreach (var factory in factories.ToList().WithProgress(bar))
        {
            var context = factory.CreateDbContext([]);
            performContext.WriteLine($"Migrating database: {context.GetType().Name}");
            await context.Database.MigrateAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        await mediator.Publish(new MigrationCompletedEvent(), cancellationToken).ConfigureAwait(false);
    }
}
