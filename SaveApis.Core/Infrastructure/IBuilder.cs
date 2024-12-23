namespace SaveApis.Core.Infrastructure;

public interface IBuilder<out TResult>
{
    TResult Build();
}
