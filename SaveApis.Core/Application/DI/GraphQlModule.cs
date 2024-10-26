using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace SaveApis.Core.Application.DI;

public class GraphQlModule<TQuery, TMutation> : Module where TQuery : class where TMutation : class
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddAuthorization();

        collection.AddGraphQLServer()
            .AddAuthorization()
            .AddQueryType<TQuery>()
            .AddMutationType<TMutation>();

        builder.Populate(collection);
    }
}