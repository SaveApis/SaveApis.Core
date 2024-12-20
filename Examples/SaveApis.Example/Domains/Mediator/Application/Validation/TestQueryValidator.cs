using FluentValidation;
using SaveApis.Example.Domains.Mediator.Application.Queries.Test;

namespace SaveApis.Example.Domains.Mediator.Application.Validation;

public class TestQueryValidator : AbstractValidator<TestQuery>
{
    public TestQueryValidator()
    {
        RuleFor(x => x.Text).NotEmpty().MaximumLength(100);
    }
}
