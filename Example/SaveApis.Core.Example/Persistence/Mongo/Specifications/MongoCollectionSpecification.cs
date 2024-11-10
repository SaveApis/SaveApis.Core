using SaveApis.Core.Example.Domain.Models.Mongo;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Collection;

namespace SaveApis.Core.Example.Persistence.Mongo.Specifications;

public class MongoCollectionSpecification : BaseMongoCollectionSpecification<MongoEntity>
{
    public override string Name => "Example";
}