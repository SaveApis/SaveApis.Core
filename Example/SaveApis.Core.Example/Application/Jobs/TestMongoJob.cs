using Hangfire;
using MongoDB.Driver;
using SaveApis.Core.Application.Events;
using SaveApis.Core.Domain.Models.Mongo;
using SaveApis.Core.Example.Domains.Models;
using SaveApis.Core.Example.Persistence.Mongo.Collection;
using SaveApis.Core.Infrastructure.Jobs;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;
using ILogger = Serilog.ILogger;

namespace SaveApis.Core.Example.Application.Jobs;

[Queue("medium")]
public class TestMongoJob(ILogger logger, IMongoCollectionFactory factory) : BaseJob<ApplicationStartedEvent>(logger)
{
    [JobDisplayName("Test mongo job")]
    public override async Task RunAsync(ApplicationStartedEvent @event, CancellationToken cancellationToken = default)
    {
        var collection = factory.Create(new TestMongoCollectionSpecification());

        var model = new ExampleMongoModel
        {
            Age = 20,
            Name = "Test"
        };
        await collection.InsertOneAsync(model, new InsertOneOptions(), cancellationToken);

        var versionCollection = factory.Create(new TestMongoVersionCollectionSpecification());

        var versionModel = new ExampleVersionMongoModel
        {
            Age = 20,
            Name = "Test",
        };
        var versionObject = new MongoVersionObject<ExampleVersionMongoModel>
        {
            Data = versionModel,
            Version = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };
        await versionCollection.InsertOneAsync(versionObject, new InsertOneOptions(), cancellationToken);
    }
}