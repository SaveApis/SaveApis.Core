using FluentResults;
using MediatR;

namespace SaveApis.Core.Common.Infrastructure.Mediator;

public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, Result<TResult>> where TCommand : ICommand<TResult>;
