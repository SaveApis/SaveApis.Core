using Autofac;
using Microsoft.AspNetCore.Builder;
using SaveApis.Core.Common.Application.Exceptions;
using SaveApis.Core.Web.Infrastructure.DI;

namespace SaveApis.Core.Web.Infrastructure.Extensions;

public static class ContainerBuilderExtensions
{
    internal static ICollection<Func<WebApplication, Task>> PreAuthenticationActions { get; } = [];
    internal static ICollection<Func<WebApplication, Task>> PostAuthenticationActions { get; } = [];

    public static ContainerBuilder WithWebModule<TModule>(this ContainerBuilder builder, params object?[] parameters) where TModule : BaseWebModule
    {
        var instance = Activator.CreateInstance(typeof(TModule), parameters);
        if (instance is not BaseWebModule module)
        {
            throw new InvalidModuleException<TModule>();
        }

        PreAuthenticationActions.Add(module.PreAuthenticationAsync);
        PostAuthenticationActions.Add(module.PostAuthenticationAsync);

        builder.RegisterModule(module);

        return builder;
    }
}
