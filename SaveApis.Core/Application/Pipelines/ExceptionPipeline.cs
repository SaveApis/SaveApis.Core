using FluentResults;
using MediatR;

namespace SaveApis.Core.Application.Pipelines;

public class ExceptionPipeline<TRequest, TResult> : IPipelineBehavior<TRequest, FluentResults.Result<TResult>>
    where TRequest : notnull
{
    public async Task<FluentResults.Result<TResult>> Handle(TRequest request, RequestHandlerDelegate<FluentResults.Result<TResult>> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            return new ExceptionalError(e);
        }
    }
}