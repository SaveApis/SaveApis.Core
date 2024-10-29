using MongoDB.Driver;
using SaveApis.Core.Domain.Models.ValueObject;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Collection;

namespace SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;

public interface IMongoCollectionFactory
{
    IMongoCollection<TModel> Create<TModel>(MongoDatabase database, string collectionName);

    IMongoCollection<TModel> Create<TModel>(BaseMongoCollectionSpecification<TModel> specification)
    {
        return Create<TModel>(specification.Database, specification.Name.ToLowerInvariant());
    }
}