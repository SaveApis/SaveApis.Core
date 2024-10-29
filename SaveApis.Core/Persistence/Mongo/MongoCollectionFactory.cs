using MongoDB.Driver;
using SaveApis.Core.Domain.Models.ValueObject;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;

namespace SaveApis.Core.Persistence.Mongo;

public class MongoCollectionFactory(IMongoDatabaseFactory factory) : IMongoCollectionFactory
{
    public IMongoCollection<TModel> Create<TModel>(MongoDatabase database, string collectionName)
    {
        var db = factory.Create(database);

        return db.GetCollection<TModel>(collectionName);
    }
}