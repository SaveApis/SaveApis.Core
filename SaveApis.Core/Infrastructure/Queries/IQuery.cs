using MediatR;
using SaveApis.Core.Domain.Models.ValueObject;

namespace SaveApis.Core.Infrastructure.Queries;

public interface IQuery<TResponse> : IRequest<FluentResults.Result<TResponse>>;

public interface ICachedQuery<TResponse> : IQuery<TResponse>
{
    CacheKey CacheKey { get; }
    TimeSpan CacheDuration { get; }
}