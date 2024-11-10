using EasyCaching.Core;
using MediatR;
using SaveApis.Core.Infrastructure.Queries;

namespace SaveApis.Core.Application.Pipelines;

public class CachedQueryPipeline<TRequest, TResponse>(IHybridCachingProvider provider)
    : IPipelineBehavior<TRequest, FluentResults.Result<TResponse>> where TRequest : notnull
{
    public async Task<FluentResults.Result<TResponse>> Handle(TRequest request,
        RequestHandlerDelegate<FluentResults.Result<TResponse>> next,
        CancellationToken cancellationToken)
    {
        if (request is not ICachedQuery<TResponse> query) return await next();

        if (await provider.ExistsAsync(query.CacheKey, cancellationToken))
        {
            var result = await provider.GetAsync<TResponse>(query.CacheKey, cancellationToken);

            if (!result.IsNull || result.HasValue) return result.Value;
        }

        var response = await next();
        await provider.SetAsync(query.CacheKey, response.Value, query.CacheDuration, cancellationToken);

        return response;
    }
}