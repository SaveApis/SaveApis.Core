using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Application.Jobs.Mongo;
using SaveApis.Core.Domain.Settings;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;
using SaveApis.Core.Persistence.Mongo;

namespace SaveApis.Core.Application.DI;

public class MongoModule(IConfiguration configuration) : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        _ = bool.TryParse(configuration["MONGO_SRV"], out var srv)
            ? srv
            : throw new ArgumentException("MONGO_SRV");
        var host = configuration["MONGO_HOST"] ?? throw new ArgumentException("MONGO_HOST");
        var port = uint.TryParse(configuration["MONGO_PORT"], out var p) ? p : 27017;
        var database = configuration["MONGO_DATABASE"] ?? throw new ArgumentException("MONGO_DATABASE");
        var user = configuration["MONGO_USER"] ?? throw new ArgumentException("MONGO_USER");
        var password = configuration["MONGO_PASSWORD"] ?? throw new ArgumentException("MONGO_PASSWORD");
        var authSource = configuration["MONGO_AUTH_SOURCE"] ?? "admin";

        var mongoSettings = new MongoSettings(srv, host, port, database, user, password, authSource);

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