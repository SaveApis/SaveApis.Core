using SaveApis.Core.Domain.Models.ValueObject;
using SaveApis.Core.Infrastructure.Events.Interfaces;

namespace SaveApis.Core.Application.Events.Cache;

public record CacheCompletedEvent(CacheKey Key) : IEvent
{
    public override string ToString()
    {
        return Key.ToString();
    }
}