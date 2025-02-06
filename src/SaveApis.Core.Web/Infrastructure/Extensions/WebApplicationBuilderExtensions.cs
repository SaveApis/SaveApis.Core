using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using SaveApis.Core.Common.Application.Helpers;
using SaveApis.Core.Common.Application.Types;
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
        var serverType = Enum.TryParse(builder.Configuration["server_type"], true, out ServerType type) ? type : ServerType.Worker;
        AssemblyHelper.RegisterAssembly(Assembly.GetExecutingAssembly());

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>((_, containerBuilder) =>
            {
                containerBuilder.RegisterInstance(AssemblyHelper).As<IAssemblyHelper>().SingleInstance();

                containerBuilder.WithWebModule<CorrelationModule>();
                containerBuilder.WithWebModule<JwtModule>(builder.Configuration);

                if (serverType is ServerType.Backend)
                {
                    containerBuilder.WithWebModule<RestModule>(AssemblyHelper.GetAssemblies());
                    containerBuilder.WithWebModule<SwaggerModule>();
                    containerBuilder.WithWebModule<HangfireDashboardModule>(serverType);
                }

                containerBuilder.RegisterCommonModules(serverType, AssemblyHelper.GetAssemblies(), builder.Configuration, additionalModules);
            });

        return builder;
    }
}
