using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Core.Application.Mediator.Behaviors;
using SaveApis.Core.Infrastructure.DI;
using SaveApis.Core.Infrastructure.Extensions;

namespace SaveApis.Core.Application.DI;

internal class MediatorModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddMediatR(serviceConfiguration =>
        {
            serviceConfiguration.RegisterServicesFromAssemblies(WebApplicationBuilderExtensions.AllAssemblies
                .ToArray());
            serviceConfiguration.AddOpenBehavior(typeof(ExceptionBehavior<,>));
            serviceConfiguration.RegisterGenericHandlers = true;
        });

        builder.Populate(collection);
    }
}
