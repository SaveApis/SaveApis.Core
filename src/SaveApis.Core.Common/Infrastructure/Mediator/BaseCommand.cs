using MediatR;
using SaveApis.Core.Common.Domains.Common.Domain.VO;
using SaveApis.Core.Common.Domains.Logging.Application.Mediator.Commands.CompleteLogEntry;
using SaveApis.Core.Common.Domains.Logging.Application.Mediator.Commands.ProgressLogEntry;
using SaveApis.Core.Common.Domains.Logging.Application.Mediator.Commands.StartLogEntry;
using SaveApis.Core.Common.Domains.Logging.Domain.Types;
using SaveApis.Core.Common.Infrastructure.Persistence.Sql.Entity;

namespace SaveApis.Core.Common.Infrastructure.Mediator;

public abstract class BaseCommand<TResult>(IMediator mediator) : ICommand<TResult>
{
    protected async Task LogAsync(LogType type, ITrackedEntity entity, Name entityName, Id? loggedBy, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new StartLogEntryCommand(type, entity.Id, entityName, loggedBy), cancellationToken).ConfigureAwait(false);

        foreach (var change in entity.GetChanges())
        {
            await mediator.Send(new ProgressLogEntryCommand(result.Value, Name.From(change.Item1), change.Item2, change.Item3), cancellationToken).ConfigureAwait(false);
        }

        await mediator.Send(new CompleteLogEntryCommand(result.Value), cancellationToken).ConfigureAwait(false);
    }
}
