using FluentResults;
using MediatR;

namespace SaveApis.Core.Common.Infrastructure.Mediator;

public interface IQuery<TResult> : IRequest<Result<TResult>>;
