using SaveApis.Core.Common.Domains.Common.Domain.VO;
using SaveApis.Core.Common.Infrastructure.Mediator;

namespace SaveApis.Core.Common.Domains.Logging.Application.Mediator.Commands.CompleteLogEntry;

public record CompleteLogEntryCommand(Id Id) : ICommand<Id>;
