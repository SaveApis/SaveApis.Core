using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Application.Pipelines;
using SaveApis.Core.Infrastructure.Events.Interfaces;
using Module = Autofac.Module;

namespace SaveApis.Core.Application.DI;

public class MediatorModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(WebApplicationBuilderExtension.Assemblies);
            configuration.AddOpenBehavior(typeof(CachedQueryPipeline<,>));
        });

        builder.Populate(collection);

        builder.RegisterAssemblyTypes(WebApplicationBuilderExtension.Assemblies)
            .Where(t => t.GetInterfaces().Any(i => i.IsAssignableTo(typeof(IEvent)))).AsImplementedInterfaces();
    }
}