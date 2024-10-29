using SaveApis.Core.Domain.Models.ValueObject;
using SaveApis.Core.Example.Domains.Models;
using SaveApis.Core.Infrastructure.Queries;

namespace SaveApis.Core.Example.Application.Queries;

public record TestCachedQuery : ICachedQuery<ExampleValidatorModel>
{
    public CacheKey CacheKey => CacheKey.Create(CacheName.Create("queries"), "test");
    public TimeSpan CacheDuration => TimeSpan.FromHours(1);
}