using SaveApis.Core.Example.Domain.Models.Mongo;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Collection;

namespace SaveApis.Core.Example.Persistence.Mongo.Specifications;

public class MongoVersionCollectionSpecification : BaseMongoVersionCollectionSpecification<VersionMongoEntity>
{
    public override string Name => "Example";
}