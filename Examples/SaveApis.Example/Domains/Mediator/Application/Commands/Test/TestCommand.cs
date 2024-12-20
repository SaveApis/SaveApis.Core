using SaveApis.Core.Infrastructure.Mediator.Commands;

namespace SaveApis.Example.Domains.Mediator.Application.Commands.Test;

public record TestCommand(string Text) : ICommand<string>;
