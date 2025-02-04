using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Core.Common.Application.Mediator.Behaviors;
using SaveApis.Core.Common.Infrastructure.DI;

namespace SaveApis.Core.Common.Application.DI;

public class MediatorModule(IEnumerable<Assembly> assemblies) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(assemblies.ToArray());
            configuration.AddOpenBehavior(typeof(ExceptionBehavior<,>));
            configuration.RegisterGenericHandlers = true;
        });

        builder.Populate(collection);
    }
}
