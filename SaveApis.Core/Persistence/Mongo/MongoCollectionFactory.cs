using MongoDB.Driver;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;

namespace SaveApis.Core.Persistence.Mongo;

public class MongoCollectionFactory(IMongoDatabaseFactory factory) : IMongoCollectionFactory
{
    public IMongoCollection<TModel> Create<TModel>(string collectionName)
    {
        var db = factory.Create();

        return db.GetCollection<TModel>(collectionName);
    }
}