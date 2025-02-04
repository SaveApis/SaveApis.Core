using SaveApis.Core.Common.Application.Hangfire.Types;

namespace SaveApis.Core.Common.Infrastructure.Hangfire.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class HangfireRecurringEventAttribute(string id, string cron, HangfireQueue queue = HangfireQueue.Medium) : Attribute
{
    public string Id { get; } = id;
    public string Cron { get; } = cron;
    public HangfireQueue Queue { get; } = queue;
}
