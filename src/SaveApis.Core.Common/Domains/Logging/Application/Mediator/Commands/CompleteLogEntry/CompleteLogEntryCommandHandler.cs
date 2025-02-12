using FluentResults;
using SaveApis.Core.Common.Domains.Common.Domain.VO;
using SaveApis.Core.Common.Domains.Logging.Domain.Types;
using SaveApis.Core.Common.Domains.Logging.Persistence.Sql;
using SaveApis.Core.Common.Infrastructure.Mediator;

namespace SaveApis.Core.Common.Domains.Logging.Application.Mediator.Commands.CompleteLogEntry;

public class CompleteLogEntryCommandHandler(ILoggingDbContextFactory factory) : ICommandHandler<CompleteLogEntryCommand, Id>
{
    public async Task<Result<Id>> Handle(CompleteLogEntryCommand request, CancellationToken cancellationToken)
    {
        await using var context = factory.Create();

        var existingEntry = await context.LogEntries.FindAsync([request.Id], cancellationToken).ConfigureAwait(false);
        if (existingEntry is null)
        {
            return Result.Fail($"Log entry with id '{request.Id}' not found!");
        }

        if (existingEntry.State == LogState.Completed)
        {
            return Result.Fail($"Log entry with id '{request.Id}' is already completed!");
        }

        existingEntry.WithState(LogState.Completed);

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return existingEntry.Id;
    }
}
