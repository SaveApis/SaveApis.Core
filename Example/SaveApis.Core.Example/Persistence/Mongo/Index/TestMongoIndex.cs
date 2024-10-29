using MongoDB.Driver;
using SaveApis.Core.Example.Domains.Models;
using SaveApis.Core.Example.Persistence.Mongo.Collection;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Collection;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Index;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;

namespace SaveApis.Core.Example.Persistence.Mongo.Index;

public class TestMongoIndex(IMongoCollectionFactory factory) : BaseMongoIndex<ExampleMongoModel>(factory)
{
    public override string Name => "test_index";
    protected override BaseMongoCollectionSpecification<ExampleMongoModel> Specification => new TestMongoCollectionSpecification();

    public override IndexKeysDefinition<ExampleMongoModel> CreateKey(IndexKeysDefinitionBuilder<ExampleMongoModel> builder)
    {
        return builder.Ascending(x => x.Name);
    }
}