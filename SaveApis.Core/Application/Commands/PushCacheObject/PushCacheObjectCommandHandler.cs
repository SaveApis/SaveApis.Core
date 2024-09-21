﻿using EasyCaching.Core;
using MediatR;
using SaveApis.Core.Application.Models.Dtos;

namespace SaveApis.Core.Application.Commands.PushCacheObject;

public class PushCacheObjectCommandHandler(IHybridCachingProvider provider)
    : IRequestHandler<PushCacheObjectCommand, PushCacheObjectResult>
{
    public async Task<PushCacheObjectResult> Handle(PushCacheObjectCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            await provider.SetAsync(request.CacheKey.ToString(), request.Value, request.Expiration,
                cancellationToken);

            return new PushCacheObjectResult(new CacheObjectDto(request.CacheKey, request.Value));

        }
        catch (Exception e)
        {
            return new PushCacheObjectResult(e);
        }
    }
}