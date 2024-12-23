namespace SaveApis.Core.Infrastructure;

public interface IBuilder<TResult>
{
    TResult Build();
    Task<TResult> BuildAsync();
}
