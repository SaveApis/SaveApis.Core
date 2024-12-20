using SaveApis.Core.Infrastructure.Mediator.Queries;

namespace SaveApis.Example.Domains.Mediator.Application.Queries.Test;

public record TestQuery(string Text) : IQuery<string>;
