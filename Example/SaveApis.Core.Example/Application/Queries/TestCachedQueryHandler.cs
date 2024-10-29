using FluentResults;
using SaveApis.Core.Example.Domains.Models;
using SaveApis.Core.Infrastructure.Queries;

namespace SaveApis.Core.Example.Application.Queries;

public class TestCachedQueryHandler : IQueryHandler<TestCachedQuery, ExampleValidatorModel>
{
    public Task<Result<ExampleValidatorModel>> Handle(TestCachedQuery request, CancellationToken cancellationToken)
    {
        var model = new ExampleValidatorModel("Test");

        return Task.FromResult(Result.Ok(model));
    }
}