using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Infrastructure.Events.Interfaces;

namespace SaveApis.Core.Application.DI;

public class MediatorModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblies(WebApplicationBuilderExtension.Assemblies));

        builder.Populate(collection);

        builder.RegisterAssemblyTypes(WebApplicationBuilderExtension.Assemblies)
            .Where(t => t.GetInterfaces().Any(i => i.IsAssignableTo(typeof(IEvent)))).AsImplementedInterfaces();
    }
}