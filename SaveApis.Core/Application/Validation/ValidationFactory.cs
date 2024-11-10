using Autofac;
using FluentValidation;
using SaveApis.Core.Infrastructure.Validation;

namespace SaveApis.Core.Application.Validation;

public class ValidationFactory(ILifetimeScope scope) : IValidationFactory
{
    public IValidator<TRequest>? GetValidator<TRequest>() where TRequest : notnull
    {
        var optional = scope.ResolveOptional<IValidator<TRequest>>();
        return optional;
    }
}