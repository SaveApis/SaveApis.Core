using Autofac;

namespace SaveApis.Core.Common.Infrastructure.DI;

/// <summary>
/// Base class to create a Module
/// </summary>
public abstract class BaseModule : Module
{
    /// <summary>
    /// Override method to enforce loading
    /// </summary>
    /// <param name="builder">Instance of the <see cref="ContainerBuilder"/></param>
    protected abstract override void Load(ContainerBuilder builder);
}
