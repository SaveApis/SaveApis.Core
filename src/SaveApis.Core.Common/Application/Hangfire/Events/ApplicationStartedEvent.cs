using SaveApis.Core.Common.Application.Types;
using SaveApis.Core.Common.Infrastructure.Hangfire.Events;

namespace SaveApis.Core.Common.Application.Hangfire.Events;

public class ApplicationStartedEvent(ServerType serverType) : IEvent
{
    public ServerType ServerType { get; } = serverType;

    public override string ToString()
    {
        return $"Application Started / {ServerType}";
    }
}
