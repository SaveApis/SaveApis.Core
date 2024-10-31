using HotChocolate;
using MediatR;
using SaveApis.Core.Domain.Models.ValueObject;
using SaveApis.Core.Example.Application.Queries.CachedExample;

namespace SaveApis.Core.Example.Application.Backend.GraphQL;

public class ExampleQuery
{
    public string GetExample()
    {
        return "Example";
    }

    public string GetExampleById(int id)
    {
        return $"Example {id}";
    }

    public async Task<string> GetExampleByQuery([Service] IMediator mediator)
    {
        var result = await mediator.Send(new Queries.Example.ExampleQuery());
        return result.Value;
    }

    public async Task<string> GetCachedExampleByQuery([Service] IMediator mediator)
    {
        var result = await mediator.Send(new CachedExampleQuery(CacheKey.Create(CacheName.Create("test"), "cache"),
            TimeSpan.FromHours(1)));
        return result.Value;
    }
}