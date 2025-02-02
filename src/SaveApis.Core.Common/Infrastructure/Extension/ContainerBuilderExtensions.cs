using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using SaveApis.Core.Common.Application.DI;
using SaveApis.Core.Common.Application.Exceptions;
using SaveApis.Core.Common.Infrastructure.DI;

namespace SaveApis.Core.Common.Infrastructure.Extension;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder WithCommonModule<TModule>(this ContainerBuilder builder, params object?[] parameters) where TModule : BaseModule
    {
        var instance = Activator.CreateInstance(typeof(TModule), parameters);
        if (instance is not BaseModule module)
        {
            throw new InvalidModuleException<TModule>();
        }

        builder.RegisterModule(module);

        return builder;
    }

    public static ContainerBuilder RegisterCommonModules(this ContainerBuilder builder, IEnumerable<Assembly> assemblies, IConfiguration configuration, Action<ContainerBuilder>? additionalModules = null)
    {
        builder.WithCommonModule<MediatorModule>(assemblies);
        builder.WithCommonModule<EfCoreModule>(assemblies);
        builder.WithCommonModule<HangfireModule>(configuration, assemblies);
        additionalModules?.Invoke(builder);

        builder.WithCommonModule<SerilogModule>();

        return builder;
    }
}
