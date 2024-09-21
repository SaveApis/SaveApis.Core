using Autofac;
using Autofac.Extensions.DependencyInjection;
using EasyCaching.Core.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace SaveApis.Core.Application.DI;

public class EasyCachingModule : Module
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
            var host = Environment.GetEnvironmentVariable("EASYCACHING_REDIS_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("EASYCACHING_REDIS_PORT") ?? "6379";
            var redisDatabase = Environment.GetEnvironmentVariable("EASYCACHING_REDIS_DATABASE") ?? "0";
            var busDatabase = Environment.GetEnvironmentVariable("EASYCACHING_REDIS_BUS_DATABASE") ?? "0";

            var endpoint = new ServerEndPoint(host, int.Parse(port));

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