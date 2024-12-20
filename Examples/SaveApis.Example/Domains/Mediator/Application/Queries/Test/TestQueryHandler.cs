using FluentResults;
using SaveApis.Core.Infrastructure.Mediator.Queries;

namespace SaveApis.Example.Domains.Mediator.Application.Queries.Test;

public class TestQueryHandler : IQueryHandler<TestQuery, string>
{
    public Task<Result<string>> Handle(TestQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Ok($"Query: {request.Text}"));
    }
}
