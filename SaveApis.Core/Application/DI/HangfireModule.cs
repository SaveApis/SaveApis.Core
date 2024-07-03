using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Infrastructure.Hangfire.Types;
using SaveApis.Core.Infrastructure.Jobs.Interfaces;

namespace SaveApis.Core.Application.DI;

public class HangfireModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddHangfire(configuration =>
        {
            var redisString = GenerateRedisConnectionString();
            configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
            configuration.UseSimpleAssemblyNameTypeSerializer();
            configuration.UseRecommendedSerializerSettings();
            configuration.UseRedisStorage(redisString);
        });

        collection.AddHangfireServer(options =>
            options.Queues = Enum.GetValues<HangfireQueues>().Select(q => q.ToString().ToLower()).ToArray());

        builder.Populate(collection);

        builder.RegisterAssemblyTypes(WebApplicationBuilderExtension.Assemblies)
            .Where(t => t.IsAssignableTo(typeof(IDashboardAuthorizationFilter)))
            .As<IDashboardAuthorizationFilter>();

        builder.RegisterAssemblyTypes(WebApplicationBuilderExtension.Assemblies)
            .Where(t => t.GetInterfaces()
                .Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == typeof(IJob<>)))
            .AsImplementedInterfaces();
    }

    private static string GenerateRedisConnectionString()
    {
        var name = Environment.GetEnvironmentVariable("HANGFIRE_REDIS_NAME");
        var host = Environment.GetEnvironmentVariable("HANGFIRE_REDIS_HOST") ?? "localhost";
        var port = Environment.GetEnvironmentVariable("HANGFIRE_REDIS_PORT") ?? "6379";
        var database = Environment.GetEnvironmentVariable("HANGFIRE_REDIS_DATABASE") ?? "0";
        var username = Environment.GetEnvironmentVariable("HANGFIRE_REDIS_USERNAME");
        var password = Environment.GetEnvironmentVariable("HANGFIRE_REDIS_PASSWORD");
        var ssl = Environment.GetEnvironmentVariable("HANGFIRE_REDIS_SSL") ?? "false";

        var stringBuilder = new StringBuilder();
        stringBuilder.Append(host).Append(':').Append(port).Append(',')
            .Append("defaultDatabase=").Append(database);
        if (!string.IsNullOrEmpty(name))
            stringBuilder.Append(',').Append("name=").Append(name);
        if (!string.IsNullOrEmpty(username))
            stringBuilder.Append(',').Append("user=").Append(username);
        if (!string.IsNullOrEmpty(password))
            stringBuilder.Append(',').Append("password=").Append(password);
        if (bool.TryParse(ssl, out var sslValue) && sslValue)
            stringBuilder.Append(',').Append("ssl=true");
        return stringBuilder.ToString();
    }
}