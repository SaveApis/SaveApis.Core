using FluentResults;
using SaveApis.Core.Infrastructure.Queries;

namespace SaveApis.Core.Example.Application.Queries.Example;

public class ExampleQueryHandler : IQueryHandler<ExampleQuery, string>
{
    public Task<Result<string>> Handle(ExampleQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Ok("Hello, World!"));
    }
}