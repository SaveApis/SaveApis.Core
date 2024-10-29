using MongoDB.Bson;

namespace SaveApis.Core.Example.Domains.Models;

public class ExampleMongoModel
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}