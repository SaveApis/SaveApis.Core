using SaveApis.Core.Domain.Models.ValueObject;
using SaveApis.Core.Example.Domains.Models;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Collection;

namespace SaveApis.Core.Example.Persistence.Mongo.Collection;

public class TestMongoVersionCollectionSpecification : BaseMongoVersionCollectionSpecification<ExampleVersionMongoModel>
{
    public override MongoDatabase Database => MongoDatabase.Create("TestDatabase");
    public override string Name => "TestCollection";
}