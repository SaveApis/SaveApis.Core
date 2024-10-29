using SaveApis.Core.Domain.Models.Mongo;

namespace SaveApis.Core.Infrastructure.Persistence.Mongo.Collection;

public abstract class BaseMongoVersionCollectionSpecification<TModel> : BaseMongoCollectionSpecification<MongoVersionObject<TModel>>;