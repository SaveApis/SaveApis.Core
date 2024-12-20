using HotChocolate;
using HotChocolate.Types;
using MediatR;
using SaveApis.Core.Infrastructure.Extensions;
using SaveApis.Example.Domains.Mediator.Application.Queries.Test;

namespace SaveApis.Example.Domains.Mediator.Application.Backend;

[QueryType]
public static class MediatorQuery
{
    public static async Task<string> TestQuery([Service] IMediator mediator, string test)
    {
        var result = await mediator.Send(new TestQuery(test)).ConfigureAwait(false);
        result.ThrowIfFailed();

        return result.Value;
    }
}
