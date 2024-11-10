using MongoDB.Driver;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Collection;

namespace SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;

public interface IMongoCollectionFactory
{
    IMongoCollection<TModel> Create<TModel>(string collectionName);

    IMongoCollection<TModel> Create<TModel>(BaseMongoCollectionSpecification<TModel> specification)
    {
        return Create<TModel>(specification.Name.ToLowerInvariant());
    }
}