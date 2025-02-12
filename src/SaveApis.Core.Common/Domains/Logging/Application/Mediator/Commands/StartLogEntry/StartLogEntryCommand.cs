using SaveApis.Core.Common.Domains.Common.Domain.VO;
using SaveApis.Core.Common.Domains.Logging.Domain.Types;
using SaveApis.Core.Common.Infrastructure.Mediator;

namespace SaveApis.Core.Common.Domains.Logging.Application.Mediator.Commands.StartLogEntry;

public record StartLogEntryCommand(LogType Type, Id AffectedEntityId, Name AffectedEntityName, Id? LoggedBy = null) : ICommand<Id>;
