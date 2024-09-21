using SaveApis.Core.Application.Models.Base;

namespace SaveApis.Core.Application.Extensions;

public static class BaseResultExtension
{
    public static void ThrowOnErrors<TModel>(this BaseResult<TModel>? result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (!result.Successful)
        {
            throw new AggregateException(result.Exceptions);
        }
    }
}