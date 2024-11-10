using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;

namespace SaveApis.Core.Persistence.Mongo;

public class MongoClientFactory(IConfiguration configuration) : IMongoClientFactory
{
    public IMongoClient Create()
    {
        _ = bool.TryParse(configuration["MONGO_SRV"], out var srv) && srv;
        _ = bool.TryParse(configuration["MONGO_RETRY_READS"], out var retryReads) && retryReads;
        _ = bool.TryParse(configuration["MONGO_RETRY_WRITES"], out var retryWrites) && retryWrites;
        var host = configuration["MONGO_HOST"] ?? "localhost";
        var port = uint.TryParse(configuration["MONGO_PORT"], out var p) ? p : 27017;
        var mechanism = configuration["MONGO_MECHANISM"] ?? "SCRAM-SHA-1";
        var authSource = configuration["MONGO_AUTH_SOURCE"] ?? "admin";
        var user = configuration["MONGO_USER"] ?? "saveapis";
        var password = configuration["MONGO_PASSWORD"] ?? "saveapis";

        var settings = new MongoClientSettings
        {
            Scheme = srv ? ConnectionStringScheme.MongoDBPlusSrv : ConnectionStringScheme.MongoDB,
            Server = new MongoServerAddress(host, (int)port),
            Credential = new MongoCredential(mechanism, new MongoExternalIdentity(authSource, user),
                new PasswordEvidence(password)),
            ApplicationName = "SaveApis",
            RetryReads = retryReads,
            RetryWrites = retryWrites
        };

        var client = new MongoClient(settings);
        return client;
    }
}