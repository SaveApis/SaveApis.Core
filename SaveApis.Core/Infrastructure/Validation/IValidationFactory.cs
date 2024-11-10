using FluentValidation;

namespace SaveApis.Core.Infrastructure.Validation;

public interface IValidationFactory
{
    IValidator<TRequest>? GetValidator<TRequest>() where TRequest : notnull;
}