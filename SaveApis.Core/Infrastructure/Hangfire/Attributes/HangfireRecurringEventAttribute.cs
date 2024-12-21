using SaveApis.Core.Application.Hangfire;

namespace SaveApis.Core.Infrastructure.Hangfire.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class HangfireRecurringEventAttribute(string id, string cronExpression, HangfireQueue queue = HangfireQueue.Medium) : Attribute
{
    public string Id { get; } = id;
    public string CronExpression { get; } = cronExpression;
    public HangfireQueue Queue { get; } = queue;
}
