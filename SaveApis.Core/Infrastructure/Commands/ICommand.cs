using MediatR;

namespace SaveApis.Core.Infrastructure.Commands;

public interface ICommand : IRequest<FluentResults.Result>;

public interface ICommand<TResult> : IRequest<FluentResults.Result<TResult>>;