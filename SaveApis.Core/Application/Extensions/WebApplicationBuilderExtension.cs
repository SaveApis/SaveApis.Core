using System.Collections.ObjectModel;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using SaveApis.Core.Application.DI;

namespace SaveApis.Core.Application.Extensions;

public static class WebApplicationBuilderExtension
{
    private static readonly Collection<Assembly> AssemblyStorage = [Assembly.GetExecutingAssembly()];
    public static readonly Collection<Action<WebApplication>> PostActions = [];
    internal static Assembly[] Assemblies => AssemblyStorage.Distinct().ToArray();


    public static WebApplicationBuilder WithAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            AssemblyStorage.Add(assembly);
        }

        return builder;
    }

    public static WebApplicationBuilder WithAutofac(this WebApplicationBuilder builder,
        Action<ContainerBuilder, IConfiguration>? register = default)
    {
        builder.Configuration.AddEnvironmentVariables();
        builder.Host
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>((_, containerBuilder) =>
            {
                register?.Invoke(containerBuilder, builder.Configuration);
                containerBuilder.RegisterModule(new SerilogModule(builder.Configuration));
                containerBuilder.RegisterModule(new MediatorModule(builder.Configuration));
                containerBuilder.RegisterModule(new HangfireModule(builder.Configuration));
                containerBuilder.RegisterModule(new JwtModule(builder.Configuration));
                containerBuilder.RegisterModule(new EasyCachingModule(builder.Configuration));

                var assemblies = Assemblies.Where(it => it != Assembly.GetExecutingAssembly()).ToArray();
                containerBuilder.RegisterAssemblyModules(assemblies);
            });

        return builder;
    }
}