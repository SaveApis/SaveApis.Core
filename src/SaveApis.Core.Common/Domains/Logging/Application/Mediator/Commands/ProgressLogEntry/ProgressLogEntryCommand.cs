using SaveApis.Core.Common.Domains.Common.Domain.VO;
using SaveApis.Core.Common.Infrastructure.Mediator;

namespace SaveApis.Core.Common.Domains.Logging.Application.Mediator.Commands.ProgressLogEntry;

public record ProgressLogEntryCommand(Id Id, Name AttributeName, string? OldValue, string? NewValue) : ICommand<Id>;
