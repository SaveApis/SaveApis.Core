using SaveApis.Core.Common.Domains.Common.Domain.VO;
using SaveApis.Core.Common.Domains.Logging.Domain.Types;

namespace SaveApis.Core.Common.Domains.Logging.Domain.Entities;

public class LogEntryEntity
{
    private LogEntryEntity(Id id, DateTime loggedAt, LogType logType, Id affectedEntityId, Name affectedEntityName, LogState state, Id? loggedBy)
    {
        Id = id;
        LoggedAt = loggedAt;
        LogType = logType;
        AffectedEntityId = affectedEntityId;
        AffectedEntityName = affectedEntityName;
        State = state;
        LoggedBy = loggedBy;
    }

    public Id Id { get; }
    public DateTime LoggedAt { get; }
    public LogType LogType { get; }

    public Id AffectedEntityId { get; }
    public Name AffectedEntityName { get; }
    public LogState State { get; private set; }

    public Id? LoggedBy { get; }

    public virtual List<LogEntryValueEntity> Values { get; set; } = [];

    public LogEntryEntity WithState(LogState state)
    {
        State = state;

        return this;
    }

    public LogEntryEntity WithValue(Name attributeName, string? oldValue, string? newValue)
    {
        Values.Add(LogEntryValueEntity.Create(attributeName, oldValue, newValue, Id));

        return this;
    }

    public static LogEntryEntity Create(LogType logType, Id affectedEntityId, Name affectedEntityName, Id? loggedBy = null)
    {
        return new LogEntryEntity(Id.From(Guid.NewGuid()), DateTime.UtcNow, logType, affectedEntityId, affectedEntityName, LogState.Started, loggedBy);
    }
}
