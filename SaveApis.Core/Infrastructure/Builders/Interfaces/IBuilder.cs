namespace SaveApis.Core.Infrastructure.Builders.Interfaces;

public interface IBuilder<TResult>
{
    Task<TResult> Build();
}