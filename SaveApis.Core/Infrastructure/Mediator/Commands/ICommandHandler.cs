using MediatR;

namespace SaveApis.Core.Infrastructure.Mediator.Commands;

public interface ICommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, FluentResults.Result<TResponse>>
    where TRequest : ICommand<TResponse>;
