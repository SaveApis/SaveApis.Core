using MediatR;

namespace SaveApis.Core.Infrastructure.Mediator.Queries;

public interface IQueryHandler<in TRequest, TResponse> : IRequestHandler<TRequest, FluentResults.Result<TResponse>>
    where TRequest : IQuery<TResponse>;
