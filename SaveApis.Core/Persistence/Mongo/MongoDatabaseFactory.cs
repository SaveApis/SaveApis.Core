using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;

namespace SaveApis.Core.Persistence.Mongo;

public class MongoDatabaseFactory(IConfiguration configuration, IMongoClientFactory factory) : IMongoDatabaseFactory
{
    public IMongoDatabase Create()
    {
        var database = configuration["MONGO_DATABASE"] ?? "saveapis";
        var client = factory.Create();

        return client.GetDatabase(database);
    }
}