using FluentResults;
using MediatR;

namespace SaveApis.Core.Common.Infrastructure.Mediator;

public interface ICommand<TResult> : IRequest<Result<TResult>>;
