using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using SaveApis.Core.Infrastructure.DI;

namespace SaveApis.Core.Infrastructure.Extensions;

public static class ContainerBuilderExtensions
{
    internal static ICollection<Action<WebApplication>> Actions { get; } = [];
    internal static ICollection<Func<WebApplication, Task>> AsyncActions { get; } = [];

    public static ContainerBuilder WithModule<TModule>(this ContainerBuilder builder, IConfiguration configuration,
        params object[] additionalArgs) where TModule : BaseModule
    {
        if (Activator.CreateInstance(typeof(TModule), [configuration, .. additionalArgs]) is not BaseModule module)
        {
            throw new InvalidOperationException("Module must inherit from BaseModule");
        }

        Actions.Add(module.PostAction);
        AsyncActions.Add(module.PostActionAsync);

        builder.RegisterModule(module);

        return builder;
    }
}
