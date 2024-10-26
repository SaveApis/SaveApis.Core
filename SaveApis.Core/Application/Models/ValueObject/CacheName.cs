namespace SaveApis.Core.Application.Models.ValueObject;

public record CacheName(string Value)
{
    public static CacheName Create(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

        return new CacheName(value);
    }

    public static implicit operator string(CacheName cacheName)
    {
        return cacheName.ToString();
    }

    public override string ToString()
    {
        return Value;
    }
}