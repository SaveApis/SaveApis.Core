using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace SaveApis.Core.Application.Extensions;

public static class WebApplicationBuilderExtension
{
    private static readonly List<Assembly> AssemblyStorage = [Assembly.GetExecutingAssembly()];
    internal static Assembly[] Assemblies => AssemblyStorage.Distinct().ToArray();


    public static WebApplicationBuilder WithAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
    {
        AssemblyStorage.AddRange(assemblies);
        return builder;
    }

    public static WebApplicationBuilder WithAutofac(this WebApplicationBuilder builder)
    {
        builder.Host
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>((_, containerBuilder) =>
                containerBuilder.RegisterAssemblyModules(Assemblies));
        return builder;
    }
}