using MongoDB.Driver;
using SaveApis.Core.Domain.Models.ValueObject;

namespace SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;

public interface IMongoDatabaseFactory
{
    IMongoDatabase Create(MongoDatabase database);
}