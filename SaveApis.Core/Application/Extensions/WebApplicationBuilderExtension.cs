using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace SaveApis.Core.Application.Extensions;

public static class WebApplicationBuilderExtension
{
    internal static List<Assembly> Assemblies { get; } = [Assembly.GetExecutingAssembly()];


    public static WebApplicationBuilder WithAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
    {
        Assemblies.AddRange(assemblies);
        return builder;
    }

    public static WebApplicationBuilder WithAutofac(this WebApplicationBuilder builder)
    {
        builder.Host
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>((_, containerBuilder) =>
                containerBuilder.RegisterAssemblyModules(Assemblies.Distinct().ToArray()));
        return builder;
    }
}