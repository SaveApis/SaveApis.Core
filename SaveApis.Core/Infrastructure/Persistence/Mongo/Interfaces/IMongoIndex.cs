namespace SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;

public interface IMongoIndex
{
    string Name { get; }
    Task Create();
}