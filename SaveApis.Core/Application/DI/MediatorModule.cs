using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Application.Pipelines;
using SaveApis.Core.Infrastructure.DI;
using SaveApis.Core.Infrastructure.Events.Interfaces;

namespace SaveApis.Core.Application.DI;

public class MediatorModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Register(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(ContainerBuilderExtension.Assemblies);
            configuration.AddOpenBehavior(typeof(CachedQueryPipeline<,>));
        });

        builder.Populate(collection);

        builder.RegisterAssemblyTypes(ContainerBuilderExtension.Assemblies)
            .Where(t => t.GetInterfaces().Any(i => i.IsAssignableTo(typeof(IEvent)))).AsImplementedInterfaces();
    }
}