using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SaveApis.Core.Domain.Settings;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;

namespace SaveApis.Core.Persistence.Mongo;

public class MongoClientFactory(IOptions<MongoSettings> options) : IMongoClientFactory
{
    public IMongoClient Create()
    {
        var settings = MongoClientSettings.FromConnectionString(options.Value);

        return new MongoClient(settings);
    }
}