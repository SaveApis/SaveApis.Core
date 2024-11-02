using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Application.Jobs.Mongo;
using SaveApis.Core.Domain.Settings;
using SaveApis.Core.Infrastructure.DI;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;
using SaveApis.Core.Persistence.Mongo;

namespace SaveApis.Core.Application.DI;

public class MongoModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Register(ContainerBuilder builder)
    {
        _ = bool.TryParse(Configuration["MONGO_SRV"], out var srv) && srv;
        var host = Configuration["MONGO_HOST"] ?? "localhost";
        var port = uint.TryParse(Configuration["MONGO_PORT"], out var p) ? p : 27017;
        var user = Configuration["MONGO_USER"] ?? "saveapis";
        var password = Configuration["MONGO_PASSWORD"] ?? "saveapis";
        var authSource = Configuration["MONGO_AUTH_SOURCE"] ?? "admin";

        var mongoSettings = new MongoSettings(srv, host, port, user, password, authSource);

        builder.RegisterType<MongoClientFactory>().As<IMongoClientFactory>();
        builder.RegisterType<MongoDatabaseFactory>().As<IMongoDatabaseFactory>();
        builder.RegisterType<MongoCollectionFactory>().As<IMongoCollectionFactory>();

        builder.RegisterInstance(Options.Create(mongoSettings)).As<IOptions<MongoSettings>>();

        builder.RegisterAssemblyTypes(WebApplicationBuilderExtension.Assemblies)
            .Where(type => type.IsAssignableTo<IMongoIndex>())
            .AsImplementedInterfaces();

        builder.RegisterType<GenerateMongoIndexesJob>().AsImplementedInterfaces();
    }
}