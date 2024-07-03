using SaveApis.Core.Infrastructure.Events.Interfaces;

namespace SaveApis.Core.Infrastructure.Events;

public abstract class BaseRecurringEvent(string id, string cronExpression) : IRecurringEvent
{
    public string Id { get; } = id;
    public string CronExpression { get; } = cronExpression;
}