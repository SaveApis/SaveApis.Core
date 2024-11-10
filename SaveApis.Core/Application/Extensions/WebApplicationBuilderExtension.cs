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
    public static readonly Collection<Action<WebApplication>> PostActions = [];

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

                var assemblies = ContainerBuilderExtension.Assemblies
                    .Where(it => it != Assembly.GetExecutingAssembly()).ToArray();
                containerBuilder.RegisterAssemblyModules(assemblies);
            });

        return builder;
    }
}