using FluentResults;
using SaveApis.Core.Common.Infrastructure.Mediator;

namespace Example.Web.Domains.Mediator.Application.Mediator.Commands.Test;

public class TestCommandHandler : ICommandHandler<TestCommand, string>
{
    public Task<Result<string>> Handle(TestCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Ok("Test"));
    }
}
