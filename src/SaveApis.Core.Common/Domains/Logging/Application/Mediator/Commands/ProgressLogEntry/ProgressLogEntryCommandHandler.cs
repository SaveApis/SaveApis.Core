using FluentResults;
using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Common.Domains.Common.Domain.VO;
using SaveApis.Core.Common.Domains.Logging.Domain.Types;
using SaveApis.Core.Common.Domains.Logging.Persistence.Sql;
using SaveApis.Core.Common.Infrastructure.Mediator;

namespace SaveApis.Core.Common.Domains.Logging.Application.Mediator.Commands.ProgressLogEntry;

public class ProgressLogEntryCommandHandler(ILoggingDbContextFactory factory) : ICommandHandler<ProgressLogEntryCommand, Id>
{
    public async Task<Result<Id>> Handle(ProgressLogEntryCommand request, CancellationToken cancellationToken)
    {
        await using var context = factory.Create();

        var existingEntry = await context.LogEntries
            .Include(e => e.Values)
            .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken)
            .ConfigureAwait(false);
        if (existingEntry is null)
        {
            return Result.Fail($"Log entry with id '{request.Id}' not found!");
        }

        if (existingEntry.State == LogState.Completed)
        {
            return Result.Fail($"Log entry with id '{request.Id}' is already completed!");
        }

        if (existingEntry.Values.Any(it => it.AttributeName == request.AttributeName))
        {
            return Result.Fail($"Log entry with id '{request.Id}' already contains value for attribute '{request.AttributeName}'!");
        }

        existingEntry.WithState(LogState.InProgress);
        existingEntry.WithValue(request.AttributeName, request.OldValue, request.NewValue);

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return existingEntry.Id;
    }
}
