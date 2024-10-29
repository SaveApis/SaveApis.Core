using SaveApis.Core.Domain.Models.ValueObject;

namespace SaveApis.Core.Infrastructure.Persistence.Mongo.Collection;

public abstract class BaseMongoCollectionSpecification<TModel>
{
    public abstract MongoDatabase Database { get; }
    public abstract string Name { get; }

    public override string ToString()
    {
        return Name.ToLowerInvariant();
    }
}