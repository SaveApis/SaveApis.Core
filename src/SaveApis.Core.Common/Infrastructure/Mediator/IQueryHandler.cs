using FluentResults;
using MediatR;

namespace SaveApis.Core.Common.Infrastructure.Mediator;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, Result<TResult>> where TQuery : IQuery<TResult>;
