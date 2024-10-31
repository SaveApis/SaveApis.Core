using MongoDB.Driver;
using SaveApis.Core.Domain.Models.Mongo;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Collection;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;
using SaveApis.Core.Persistence.Mongo.Extensions;

namespace SaveApis.Core.Infrastructure.Persistence.Mongo.Index;

public abstract class BaseMongoVersionIndex<TModel>(IMongoCollectionFactory factory) : IMongoIndex
{
    public abstract string Name { get; }
    protected abstract BaseMongoVersionCollectionSpecification<TModel> Specification { get; }

    public abstract IndexKeysDefinition<MongoVersionObject<TModel>> CreateKey(IndexKeysDefinitionBuilder<MongoVersionObject<TModel>> builder);

    public async Task Create()
    {
        var collection = factory.Create(Specification);

        var indexCursor = await collection.Indexes.ListAsync();
        var indexes = await indexCursor.ToListAsync();

        if (indexes.Any(i => i["name"].AsString.Equals(Name, StringComparison.InvariantCultureIgnoreCase)))
        {
            return;
        }

        var key = CreateKey(collection.IndexKeys());
        var indexOptions = new CreateIndexOptions
        {
            Name = Name,
            Hidden = false
        };

        await collection.Indexes.CreateOneAsync(new CreateIndexModel<MongoVersionObject<TModel>>(key, indexOptions));
    }
}