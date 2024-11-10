using MongoDB.Driver;

namespace SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;

public interface IMongoDatabaseFactory
{
    IMongoDatabase Create();
}