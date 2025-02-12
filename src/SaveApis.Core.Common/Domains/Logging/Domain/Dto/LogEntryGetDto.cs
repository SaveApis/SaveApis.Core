using SaveApis.Core.Common.Domains.Common.Domain.VO;
using SaveApis.Core.Common.Domains.Logging.Domain.Types;

namespace SaveApis.Core.Common.Domains.Logging.Domain.Dto;

public class LogEntryGetDto
{
    public Id Id { get; set; } = Id.Empty;
    public DateTime LoggedAt { get; set; } = DateTime.MinValue;
    public LogType LogType { get; set; } = LogType.Create;

    public Id AffectedEntityId { get; set; } = Id.Empty;
    public Name AffectedEntityName { get; set; } = Name.Empty;
    public LogState State { get; set; } = LogState.Started;

    public Id? LoggedBy { get; set; }

    public List<LogEntryValueGetDto> Values { get; set; } = [];
}
