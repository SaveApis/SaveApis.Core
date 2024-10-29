using FluentValidation;
using SaveApis.Core.Example.Domains.Models;

namespace SaveApis.Core.Example.Application.Validator;

public class ExampleValidatorModelValidator : AbstractValidator<ExampleValidatorModel>
{
    public ExampleValidatorModelValidator()
    {
        RuleFor(model => model.Name).NotNull().NotEmpty();
    }
}