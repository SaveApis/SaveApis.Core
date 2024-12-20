using FluentValidation;
using SaveApis.Example.Domains.Mediator.Application.Commands.Test;

namespace SaveApis.Example.Domains.Mediator.Application.Validation;

public class TestCommandValidator : AbstractValidator<TestCommand>
{
    public TestCommandValidator()
    {
        RuleFor(x => x.Text).NotEmpty().MaximumLength(100);
    }
}
