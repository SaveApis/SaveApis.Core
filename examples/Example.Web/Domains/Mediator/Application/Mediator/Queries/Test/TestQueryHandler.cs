using FluentResults;
using SaveApis.Core.Common.Infrastructure.Mediator;

namespace Example.Web.Domains.Mediator.Application.Mediator.Queries.Test;

public class TestQueryHandler : IQueryHandler<TestQuery, string>
{
    public Task<Result<string>> Handle(TestQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Ok("Test"));
    }
}
