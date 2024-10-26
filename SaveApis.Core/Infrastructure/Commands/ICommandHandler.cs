using MediatR;

namespace SaveApis.Core.Infrastructure.Commands;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, FluentResults.Result> where TCommand : ICommand;

public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, FluentResults.Result<TResult>>
    where TCommand : ICommand<TResult>;