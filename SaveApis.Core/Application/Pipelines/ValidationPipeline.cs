using FluentValidation;
using MediatR;
using SaveApis.Core.Infrastructure.Validation;

namespace SaveApis.Core.Application.Pipelines;

public class ValidationPipeline<TRequest, TResult>(IValidationFactory factory)
    : IPipelineBehavior<TRequest, TResult> where TRequest : notnull
{
    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        var validator = factory.GetValidator<TRequest>();
        if (validator is null)
        {
            throw new InvalidOperationException($"No validator found for '{typeof(TRequest).Name}'");
        }

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        return await next();
    }
}