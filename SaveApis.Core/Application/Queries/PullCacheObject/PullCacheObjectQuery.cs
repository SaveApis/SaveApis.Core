using MediatR;
using SaveApis.Core.Application.Models.ValueObject;

namespace SaveApis.Core.Application.Queries.PullCacheObject;

public record PullCacheObjectQuery(CacheKey CacheKey) : IRequest<PullCacheObjectResult>;