namespace SaveApis.Core.Application.Models.Base;

public class BaseResult<TModel>
{
    public IReadOnlyList<Exception> Exceptions { get; }
    public TModel Result { get; }
    public bool Successful => Exceptions.Count == 0;

    public BaseResult(TModel result)
    {
        Result = result;
        Exceptions = [];
    }

    public BaseResult(params Exception[] exceptions)
    {
        Exceptions = [.. exceptions];
        Result = default!;
    }
}