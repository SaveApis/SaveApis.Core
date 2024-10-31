using MongoDB.Driver;
using SaveApis.Core.Example.Domain.Models.Mongo;
using SaveApis.Core.Example.Persistence.Mongo.Specifications;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Collection;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Index;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;

namespace SaveApis.Core.Example.Persistence.Mongo.Index;

public class MongoIndex(IMongoCollectionFactory factory) : BaseMongoIndex<MongoEntity>(factory)
{
    public override string Name => "mongo_entity_name";
    protected override BaseMongoCollectionSpecification<MongoEntity> Specification =>
        new MongoCollectionSpecification();
    public override IndexKeysDefinition<MongoEntity> CreateKey(IndexKeysDefinitionBuilder<MongoEntity> builder)
    {
        return builder.Ascending(x => x.Name);
    }
}