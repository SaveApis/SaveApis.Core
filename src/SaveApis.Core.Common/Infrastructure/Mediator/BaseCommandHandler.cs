using FluentResults;
using MediatR;
using SaveApis.Core.Common.Domains.Common.Domain.VO;
using SaveApis.Core.Common.Domains.Logging.Application.Mediator.Commands.CompleteLogEntry;
using SaveApis.Core.Common.Domains.Logging.Application.Mediator.Commands.ProgressLogEntry;
using SaveApis.Core.Common.Domains.Logging.Application.Mediator.Commands.StartLogEntry;
using SaveApis.Core.Common.Domains.Logging.Domain.Types;
using SaveApis.Core.Common.Infrastructure.Persistence.Sql.Entity;

namespace SaveApis.Core.Common.Infrastructure.Mediator;

public abstract class BaseCommandHandler<TCommand, TResult>(IMediator mediator) : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    public abstract Task<Result<TResult>> Handle(TCommand request, CancellationToken cancellationToken);

    protected async Task LogAsync(ITrackedEntity entity, LogType type, string entityName, Id? loggedBy = null)
    {
        var result = await mediator.Send(new StartLogEntryCommand(type, entity.Id, Name.From(entityName), loggedBy)).ConfigureAwait(false);

        foreach (var change in entity.GetChanges())
        {
            await mediator.Send(new ProgressLogEntryCommand(result.Value, Name.From(change.Item1), change.Item2, change.Item3)).ConfigureAwait(false);
        }

        await mediator.Send(new CompleteLogEntryCommand(result.Value)).ConfigureAwait(false);
    }
}
