using FluentResults;
using MediatR;

namespace SaveApis.Core.Infrastructure.Queries;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, FluentResults.Result<TResponse>>
    where TQuery : IQuery<TResponse>;