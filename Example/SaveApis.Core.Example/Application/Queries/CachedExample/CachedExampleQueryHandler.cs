using FluentResults;
using SaveApis.Core.Infrastructure.Queries;

namespace SaveApis.Core.Example.Application.Queries.CachedExample;

public class CachedExampleQueryHandler : IQueryHandler<CachedExampleQuery, string>
{
    public Task<Result<string>> Handle(CachedExampleQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Ok("Hello, Cached World!"));
    }
}