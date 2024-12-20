using FluentResults;

namespace SaveApis.Core.Infrastructure.Extensions;

public static class ResultExtensions
{
    public static void ThrowIfFailed(this ResultBase result)
    {
        if (result.IsSuccess)
        {
            return;
        }

        var aggregateException = result.Exceptions();
        if (aggregateException.InnerExceptions.Count > 0)
        {
            throw aggregateException;
        }

        throw new InvalidOperationException($"One or more errors occurred. {result}");
    }

    private static AggregateException Exceptions(this ResultBase result)
    {
        var exceptionalErrors = result.Errors.SelectMany(err => err.Reasons.OfType<ExceptionalError>())
            .Select(ex => ex.Exception);

        return new AggregateException($"One or more errors occured: {result}", exceptionalErrors);
    }
}
