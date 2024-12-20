using MediatR;

namespace SaveApis.Core.Infrastructure.Mediator.Commands;

public interface ICommand<TResponse> : IRequest<FluentResults.Result<TResponse>>;
