using SaveApis.Core.Application.Models.ValueObject;
using SaveApis.Core.Example.Application.Models;
using SaveApis.Core.Infrastructure.Queries;

namespace SaveApis.Core.Example.Application.Queries;

public record TestCachedQuery : ICachedQuery<ExampleValidatorModel>
{
    public CacheKey CacheKey => CacheKey.Create(CacheName.Create("queries"), "test");
    public TimeSpan CacheDuration => TimeSpan.FromHours(1);
}