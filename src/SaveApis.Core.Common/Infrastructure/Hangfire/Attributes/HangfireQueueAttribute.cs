using SaveApis.Core.Common.Application.Hangfire.Types;

namespace SaveApis.Core.Common.Infrastructure.Hangfire.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class HangfireQueueAttribute(HangfireQueue queue) : Attribute
{
    public HangfireQueue Queue { get; } = queue;
}
