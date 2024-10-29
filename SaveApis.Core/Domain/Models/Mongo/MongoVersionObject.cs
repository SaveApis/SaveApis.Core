using MongoDB.Bson;

namespace SaveApis.Core.Domain.Models.Mongo;

public class MongoVersionObject<TModel>
{
    public ObjectId Id { get; set; }
    public long Version { get; set; }
    public TModel Data { get; set; }
}