using HotChocolate;
using HotChocolate.Types;
using MediatR;
using SaveApis.Core.Infrastructure.Extensions;
using SaveApis.Example.Domains.Mediator.Application.Commands.Test;

namespace SaveApis.Example.Domains.Mediator.Application.Backend;

[MutationType]
public static class MediatorMutation
{
    public static async Task<string> TestCommand([Service] IMediator mediator, string test)
    {
        var result = await mediator.Send(new TestCommand(test)).ConfigureAwait(false);
        result.ThrowIfFailed();

        return result.Value;
    }
}
