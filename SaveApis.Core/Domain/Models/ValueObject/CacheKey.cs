namespace SaveApis.Core.Domain.Models.ValueObject;

public record CacheKey(CacheName CacheName, string Key)
{
    public static CacheKey Create(CacheName cacheName, string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        return new CacheKey(cacheName, key);
    }

    public CacheKey Append(string key)
    {
        return Create(CacheName, $"{Key}:{key}");
    }

    public static implicit operator string(CacheKey cacheKey)
    {
        return cacheKey.ToString();
    }

    public override string ToString()
    {
        return $"{CacheName}:{Key}";
    }
}