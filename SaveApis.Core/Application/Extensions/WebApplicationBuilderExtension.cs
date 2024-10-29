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

    public static WebApplicationBuilder WithAutofac<TQuery, TMutation>(this WebApplicationBuilder builder,
        Action<ContainerBuilder, IConfiguration>? register = default) where TQuery : class where TMutation : class
    {
        builder.Configuration.AddEnvironmentVariables();
        builder.Host
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>((_, containerBuilder) =>
            {
                register?.Invoke(containerBuilder, builder.Configuration);
                containerBuilder.RegisterModule(new JwtModule(builder.Configuration));
                containerBuilder.RegisterModule(new SerilogModule(builder.Configuration));
                containerBuilder.RegisterModule(new HangfireModule(builder.Configuration));
                containerBuilder.RegisterModule<MediatorModule>();
                containerBuilder.RegisterModule(new EasyCachingModule(builder.Configuration));
                containerBuilder.RegisterModule<SwaggerModule>();
                containerBuilder.RegisterModule<GraphQlModule<TQuery, TMutation>>();

                var assemblies = Assemblies.Where(it => it != Assembly.GetExecutingAssembly()).ToArray();
                containerBuilder.RegisterAssemblyModules(assemblies);
            });

        return builder;
    }
}