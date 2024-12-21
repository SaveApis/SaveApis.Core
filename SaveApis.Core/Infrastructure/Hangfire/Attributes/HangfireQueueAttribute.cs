using SaveApis.Core.Application.Hangfire;

namespace SaveApis.Core.Infrastructure.Hangfire.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class HangfireQueueAttribute(HangfireQueue queue = HangfireQueue.Medium) : Attribute
{
    public HangfireQueue Queue { get; } = queue;
}
