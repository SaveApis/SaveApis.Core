using SaveApis.Core.Common.Application.Hangfire.Types;
using SaveApis.Core.Common.Infrastructure.Hangfire.Events;

namespace SaveApis.Core.Common.Application.Hangfire.Events;

public class ApplicationStartedEvent(HangfireServerType serverType) : IEvent
{
    public HangfireServerType ServerType { get; } = serverType;

    public override string ToString()
    {
        return $"Application Started / {ServerType}";
    }
}
