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
    internal static Assembly[] Assemblies => AssemblyStorage.Distinct().ToArray();


    public static WebApplicationBuilder WithAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            AssemblyStorage.Add(assembly);
        }

        return builder;
    }

    public static WebApplicationBuilder WithAutofac<TQuery, TMutation>(this WebApplicationBuilder builder, IConfiguration configuration,
        Action<ContainerBuilder>? register = default) where TQuery : class where TMutation : class
    {
        builder.Configuration.AddEnvironmentVariables();
        builder.Host
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>((_, containerBuilder) =>
            {
                register?.Invoke(containerBuilder);
                containerBuilder.RegisterModule(new JwtModule(configuration));
                containerBuilder.RegisterModule(new EfCoreModule(configuration));
                containerBuilder.RegisterModule(new SerilogModule(configuration));
                containerBuilder.RegisterModule(new HangfireModule(configuration));
                containerBuilder.RegisterModule<MediatorModule>();
                containerBuilder.RegisterModule(new EasyCachingModule(configuration));
                containerBuilder.RegisterModule<SwaggerModule>();
                containerBuilder.RegisterModule<GraphQlModule<TQuery, TMutation>>();

                var assemblies = Assemblies.Where(it => it != Assembly.GetExecutingAssembly()).ToArray();
                containerBuilder.RegisterAssemblyModules(assemblies);
            });

        return builder;
    }
}