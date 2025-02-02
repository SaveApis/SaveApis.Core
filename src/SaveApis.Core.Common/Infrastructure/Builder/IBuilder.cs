namespace SaveApis.Core.Common.Infrastructure.Builder;

/// <summary>
/// Base interface for all builders
/// </summary>
/// <typeparam name="TType">Type which got build</typeparam>
public interface IBuilder<TType>
{
    /// <summary>
    /// Method to build the object synchronously
    /// </summary>
    /// <returns>Instance of the type</returns>
    TType Build();

    /// <summary>
    /// Method to build the object asynchronously
    /// </summary>
    /// <returns>Instance of the type</returns>
    Task<TType> BuildAsync();
}
