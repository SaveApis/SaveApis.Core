using SaveApis.Core.Domain.Models.ValueObject;
using SaveApis.Core.Infrastructure.Queries;

namespace SaveApis.Core.Example.Application.Queries.CachedExample;

public record CachedExampleQuery(CacheKey CacheKey, TimeSpan CacheDuration) : ICachedQuery<string>;