using MongoDB.Bson;

namespace SaveApis.Core.Example.Domain.Models.Mongo;

public class MongoEntity
{
    public ObjectId Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}