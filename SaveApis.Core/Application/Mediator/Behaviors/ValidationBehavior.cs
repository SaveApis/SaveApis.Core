using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace SaveApis.Core.Application.Mediator.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, FluentResults.Result<TResponse>> where TRequest : notnull
{
    public async Task<FluentResults.Result<TResponse>> Handle(TRequest request,
        RequestHandlerDelegate<FluentResults.Result<TResponse>> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return new ExceptionalError(new InvalidOperationException(
                $"No validators found for request of type '{request.GetType().Name}'!"));
        }

        var failures = new List<ValidationFailure>();

        foreach (var validator in validators)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);
            if (!validationResult.IsValid)
            {
                failures.AddRange(validationResult.Errors);
            }
        }

        if (failures.Count != 0)
        {
            return new ExceptionalError(new ValidationException(failures));
        }

        return await next().ConfigureAwait(false);
    }
}
