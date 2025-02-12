using SaveApis.Core.Common.Domains.Common.Domain.VO;
using SaveApis.Core.Common.Domains.Logging.Domain.Entities;
using SaveApis.Core.Common.Infrastructure.Mediator;

namespace SaveApis.Core.Common.Domains.Logging.Application.Mediator.Queries;

public record GetLogEntriesByAffectedIdentifier(Id Id) : IQuery<IEnumerable<LogEntryEntity>>;
