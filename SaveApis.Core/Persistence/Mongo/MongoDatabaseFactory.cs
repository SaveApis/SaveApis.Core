using MongoDB.Driver;
using SaveApis.Core.Domain.Models.ValueObject;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;

namespace SaveApis.Core.Persistence.Mongo;

public class MongoDatabaseFactory(IMongoClientFactory factory) : IMongoDatabaseFactory
{
    public IMongoDatabase Create(MongoDatabase database)
    {
        var client = factory.Create();

        return client.GetDatabase(database);
    }
}