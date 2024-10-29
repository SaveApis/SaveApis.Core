using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Infrastructure.Hangfire.Types;
using SaveApis.Core.Infrastructure.Jobs.Interfaces;

namespace SaveApis.Core.Application.DI;

public class HangfireModule(IConfiguration configuration) : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddHangfire(config =>
        {
            var redisString = GenerateRedisConnectionString();
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
            config.UseSimpleAssemblyNameTypeSerializer();
            config.UseRecommendedSerializerSettings();
            config.UseRedisStorage(redisString);
        });

        collection.AddHangfireServer(options =>
            options.Queues = Enum.GetValues<HangfireQueues>().Select(q => q.ToString().ToLower()).ToArray());

        builder.Populate(collection);

        builder.RegisterAssemblyTypes(WebApplicationBuilderExtension.Assemblies)
            .Where(t => t.IsAssignableTo(typeof(IDashboardAuthorizationFilter)))
            .As<IDashboardAuthorizationFilter>();

        builder.RegisterAssemblyTypes(WebApplicationBuilderExtension.Assemblies)
            .Where(t =>
            {
                var interfaces = t.GetInterfaces();
                var isJob = interfaces.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == typeof(IJob<>));
                var isModuleJob = interfaces.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == typeof(IModuleSpecifiedJob<>));

                return isJob && !isModuleJob;
            })
            .AsImplementedInterfaces();
    }

    private string GenerateRedisConnectionString()
    {
        var name = configuration["HANGFIRE_REDIS_NAME"] ?? throw new ArgumentException("HANGFIRE_REDIS_NAME");
        var host = configuration["HANGFIRE_REDIS_HOST"] ?? throw new ArgumentException("HANGFIRE_REDIS_HOST");
        var port = configuration["HANGFIRE_REDIS_PORT"] ?? throw new ArgumentException("HANGFIRE_REDIS_PORT");
        var database = configuration["HANGFIRE_REDIS_DATABASE"] ?? throw new ArgumentException("HANGFIRE_REDIS_DATABASE");
        var username = configuration["HANGFIRE_REDIS_USERNAME"] ?? string.Empty;
        var password = configuration["HANGFIRE_REDIS_PASSWORD"] ?? string.Empty;
        var ssl = configuration["HANGFIRE_REDIS_SSL"] ?? "false";

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