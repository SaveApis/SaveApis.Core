using MediatR;
using SaveApis.Core.Application.Models.ValueObject;

namespace SaveApis.Core.Application.Commands.PushCacheObject;

public record PushCacheObjectCommand(CacheKey CacheKey, object Value, TimeSpan Expiration) : IRequest<PushCacheObjectResult>;