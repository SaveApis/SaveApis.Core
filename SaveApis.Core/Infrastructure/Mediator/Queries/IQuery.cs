using MediatR;

namespace SaveApis.Core.Infrastructure.Mediator.Queries;

public interface IQuery<TResponse> : IRequest<FluentResults.Result<TResponse>>;
