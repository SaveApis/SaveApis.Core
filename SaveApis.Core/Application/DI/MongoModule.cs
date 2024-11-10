using Autofac;
using Microsoft.Extensions.Configuration;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Application.Jobs.Mongo;
using SaveApis.Core.Infrastructure.DI;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;
using SaveApis.Core.Persistence.Mongo;

namespace SaveApis.Core.Application.DI;

public class MongoModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Register(ContainerBuilder builder)
    {
        builder.RegisterType<MongoClientFactory>().As<IMongoClientFactory>();
        builder.RegisterType<MongoDatabaseFactory>().As<IMongoDatabaseFactory>();
        builder.RegisterType<MongoCollectionFactory>().As<IMongoCollectionFactory>();

        builder.RegisterAssemblyTypes(ContainerBuilderExtension.Assemblies)
            .Where(type => type.IsAssignableTo<IMongoIndex>())
            .AsImplementedInterfaces();

        builder.RegisterType<GenerateMongoIndexesJob>().AsImplementedInterfaces();
    }
}