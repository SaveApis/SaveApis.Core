using MongoDB.Driver;

namespace SaveApis.Core.Persistence.Mongo.Extensions;

public static class MongoCollectionExtension
{
    public static FilterDefinitionBuilder<TModel> Filter<TModel>(this IMongoCollection<TModel> _)
    {
        return Builders<TModel>.Filter;
    }

    public static UpdateDefinitionBuilder<TModel> Update<TModel>(this IMongoCollection<TModel> _)
    {
        return Builders<TModel>.Update;
    }

    public static SortDefinitionBuilder<TModel> Sort<TModel>(this IMongoCollection<TModel> _)
    {
        return Builders<TModel>.Sort;
    }

    public static ProjectionDefinitionBuilder<TModel> Projection<TModel>(this IMongoCollection<TModel> _)
    {
        return Builders<TModel>.Projection;
    }

    public static IndexKeysDefinitionBuilder<TModel> IndexKeys<TModel>(this IMongoCollection<TModel> _)
    {
        return Builders<TModel>.IndexKeys;
    }
}