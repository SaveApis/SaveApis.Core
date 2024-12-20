using FluentResults;
using MediatR;

namespace SaveApis.Core.Application.Mediator.Behaviors;

public class ExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, FluentResults.Result<TResponse>>
    where TRequest : notnull
{
    public async Task<FluentResults.Result<TResponse>> Handle(TRequest request,
        RequestHandlerDelegate<FluentResults.Result<TResponse>> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next().ConfigureAwait(false);
        }
        catch (Exception e)
        {
            return new ExceptionalError(e);
        }
    }
}
