using FluentValidation;

namespace SaveApis.Core.Example.Application.Queries.CachedExample.Validators;

public class CacheExampleQueryValidator : AbstractValidator<CachedExampleQuery>
{
    public CacheExampleQueryValidator()
    {
        RuleFor(e => e.CacheKey).NotNull();
        RuleFor(e => e.CacheKey.Key).NotEmpty();
        RuleFor(e => e.CacheDuration).GreaterThan(TimeSpan.Zero);
    }
}