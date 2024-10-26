using Autofac;
using Autofac.Extensions.DependencyInjection;
using EasyCaching.Core.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SaveApis.Core.Application.DI;

public class EasyCachingModule(IConfiguration configuration) : Module
{
    private const string Json = "json";
    private const string InMemory = "inmemory";
    private const string Redis = "redis";
    private const string Hybrid = "hybrid";

    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddEasyCaching(options =>
        {
            var host = configuration["EASYCACHING_REDIS_HOST"] ??
                       throw new ArgumentException("EASYCACHING_REDIS_HOST");
            var port = configuration["EASYCACHING_REDIS_PORT"] ??
                       throw new ArgumentException("EASYCACHING_REDIS_PORT");
            var redisDatabase = configuration["EASYCACHING_REDIS_DATABASE"] ??
                                throw new ArgumentException("EASYCACHING_REDIS_DATABASE");
            var busDatabase = configuration["EASYCACHING_REDIS_BUS_DATABASE"] ??
                              throw new ArgumentException("EASYCACHING_REDIS_BUS_DATABASE");

            var endpoint = new ServerEndPoint(host, int.TryParse(port, out var p)
                ? p
                : throw new ArgumentException("EASYCACHING_REDIS_PORT"));

            options.WithJson(Json);
            options.UseInMemory(InMemory);
            options.UseRedis(config =>
            {
                config.DBConfig.Endpoints.Add(endpoint);
                config.DBConfig.Database = int.Parse(redisDatabase);
                config.SerializerName = Json;
            }, Redis);

            options.UseHybrid(config =>
                {
                    config.TopicName = "SaveApis";
                    config.EnableLogging = true;

                    config.LocalCacheProviderName = InMemory;
                    config.DistributedCacheProviderName = Redis;
                }, Hybrid)
                .WithRedisBus(config =>
                {
                    config.Endpoints.Add(endpoint);
                    config.Database = int.Parse(busDatabase);

                    config.SerializerName = Json;
                });
        });

        builder.Populate(collection);
    }
}