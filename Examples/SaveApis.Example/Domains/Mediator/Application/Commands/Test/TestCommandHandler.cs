using FluentResults;
using SaveApis.Core.Infrastructure.Mediator.Commands;

namespace SaveApis.Example.Domains.Mediator.Application.Commands.Test;

public class TestCommandHandler : ICommandHandler<TestCommand, string>
{
    public Task<Result<string>> Handle(TestCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Ok($"Command: {request.Text}"));
    }
}
