using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaveApis.Core.Application.DI;

namespace SaveApis.Core.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static IReadOnlyCollection<Assembly> CustomAssemblies { get; private set; } = [];
    public static IReadOnlyCollection<Assembly> AllAssemblies => [Assembly.GetExecutingAssembly(), .. CustomAssemblies];

    public static WebApplicationBuilder WithAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
    {
        CustomAssemblies = assemblies;
        return builder;
    }

    public static WebApplicationBuilder AddSaveApis(this WebApplicationBuilder builder,
        Action<IRequestExecutorBuilder> configureGraphQl,
        Action<ContainerBuilder, IConfiguration>? additionalModules = null)
    {
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                containerBuilder.WithModule<FluentValidationModule>(builder.Configuration);
                containerBuilder.WithModule<MediatorModule>(builder.Configuration);
                containerBuilder.WithModule<HangfireModule>(builder.Configuration);
                containerBuilder.WithModule<CorrelationModule>(builder.Configuration);
                additionalModules?.Invoke(containerBuilder, builder.Configuration);

                // Register serilog at the end to ensure it's the last module registered and overwrites EVERY other logger
                containerBuilder.WithModule<SerilogModule>(builder.Configuration);
            });

        var requestExecutorBuilder = builder.AddGraphQL().AddSorting().AddFiltering();
        configureGraphQl(requestExecutorBuilder);

        return builder;
    }
}
