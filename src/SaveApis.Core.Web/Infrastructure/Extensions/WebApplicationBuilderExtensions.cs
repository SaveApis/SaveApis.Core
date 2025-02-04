using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using SaveApis.Core.Common.Application.Helpers;
using SaveApis.Core.Common.Infrastructure.Extension;
using SaveApis.Core.Common.Infrastructure.Helpers;
using SaveApis.Core.Web.Application.DI;

namespace SaveApis.Core.Web.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
    private static readonly AssemblyHelper AssemblyHelper = new AssemblyHelper();

    public static WebApplicationBuilder WithAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
    {
        AssemblyHelper.RegisterAssemblies(assemblies);

        return builder;
    }

    public static WebApplicationBuilder WithSaveApis(this WebApplicationBuilder builder, Action<ContainerBuilder>? additionalModules = null)
    {
        AssemblyHelper.RegisterAssembly(Assembly.GetExecutingAssembly());

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>((_, containerBuilder) =>
            {
                containerBuilder.RegisterInstance(AssemblyHelper).As<IAssemblyHelper>().SingleInstance();

                containerBuilder.WithWebModule<CorrelationModule>();
                containerBuilder.WithWebModule<RestModule>(AssemblyHelper.GetAssemblies());
                containerBuilder.WithWebModule<JwtModule>(builder.Configuration);
                containerBuilder.WithWebModule<HangfireDashboardModule>(builder.Configuration);
                containerBuilder.WithWebModule<SwaggerModule>();

                containerBuilder.RegisterCommonModules(AssemblyHelper.GetAssemblies(), builder.Configuration, additionalModules);
            });

        return builder;
    }
}
