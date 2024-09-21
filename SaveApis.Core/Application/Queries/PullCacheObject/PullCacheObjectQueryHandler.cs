using EasyCaching.Core;
using MediatR;
using SaveApis.Core.Application.Exceptions;
using SaveApis.Core.Application.Models.Dtos;

namespace SaveApis.Core.Application.Queries.PullCacheObject;

public class PullCacheObjectQueryHandler(IHybridCachingProvider provider)
    : IRequestHandler<PullCacheObjectQuery, PullCacheObjectResult>
{
    public async Task<PullCacheObjectResult> Handle(PullCacheObjectQuery request,
        CancellationToken cancellationToken)
    {
        var cacheResult = await provider.GetAsync<object>(request.CacheKey.ToString(), cancellationToken);
        if (cacheResult.IsNull || !cacheResult.HasValue)
        {
            return new PullCacheObjectResult(new CacheObjectNotFoundException(request.CacheKey));
        }

        return new PullCacheObjectResult(new CacheObjectDto(request.CacheKey, cacheResult.Value));
    }
}