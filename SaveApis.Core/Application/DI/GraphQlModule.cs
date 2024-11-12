using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Core.Infrastructure.DI;

namespace SaveApis.Core.Application.DI;

public class GraphQlModule<TQuery, TMutation>(IConfiguration configuration)
    : BaseModule(configuration) where TQuery : class where TMutation : class
{
    protected override void Register(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddAuthorization();

        collection.AddGraphQLServer()
            .AddAuthorization()
            .AddQueryType<TQuery>()
            .AddMutationType<TMutation>()
            .AddFiltering()
            .AddSorting();

        builder.Populate(collection);
    }

    protected override void PostAction(WebApplication application)
    {
        application.MapGraphQL();
    }
}