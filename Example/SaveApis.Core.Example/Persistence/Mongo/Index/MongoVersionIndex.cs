using MongoDB.Driver;
using SaveApis.Core.Domain.Models.Mongo;
using SaveApis.Core.Example.Domain.Models.Mongo;
using SaveApis.Core.Example.Persistence.Mongo.Specifications;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Collection;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Index;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;

namespace SaveApis.Core.Example.Persistence.Mongo.Index;

public class MongoVersionIndex(IMongoCollectionFactory factory) : BaseMongoVersionIndex<VersionMongoEntity>(factory)
{
    public override string Name => "version_mongo_entity_name";

    protected override BaseMongoVersionCollectionSpecification<VersionMongoEntity> Specification =>
        new MongoVersionCollectionSpecification();
    public override IndexKeysDefinition<MongoVersionObject<VersionMongoEntity>> CreateKey(IndexKeysDefinitionBuilder<MongoVersionObject<VersionMongoEntity>> builder)
    {
        return builder.Ascending(x => x.Data.Name);
    }
}