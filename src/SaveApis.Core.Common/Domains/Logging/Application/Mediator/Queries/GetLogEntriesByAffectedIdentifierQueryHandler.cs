using FluentResults;
using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Common.Domains.Logging.Domain.Entities;
using SaveApis.Core.Common.Domains.Logging.Persistence.Sql;
using SaveApis.Core.Common.Infrastructure.Mediator;

namespace SaveApis.Core.Common.Domains.Logging.Application.Mediator.Queries;

public class GetLogEntriesByAffectedIdentifierQueryHandler(ILoggingDbContextFactory factory) : IQueryHandler<GetLogEntriesByAffectedIdentifier, IEnumerable<LogEntryEntity>>
{
    public async Task<Result<IEnumerable<LogEntryEntity>>> Handle(GetLogEntriesByAffectedIdentifier request, CancellationToken cancellationToken)
    {
        await using var context = factory.Create();

        return await context
            .LogEntries
            .Include(e => e.Values)
            .Where(it => it.AffectedEntityId == request.Id)
            .OrderByDescending(it => it.LoggedAt)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
