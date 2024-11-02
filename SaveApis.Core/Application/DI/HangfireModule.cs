using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Infrastructure.DI;
using SaveApis.Core.Infrastructure.Hangfire.Types;
using SaveApis.Core.Infrastructure.Jobs.Interfaces;

namespace SaveApis.Core.Application.DI;

public class HangfireModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Register(ContainerBuilder builder)
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

    protected override void PostAction(WebApplication application)
    {
        var dashboardAuthorizationFilters = application.Services.CreateScope().ServiceProvider
            .GetServices<IDashboardAuthorizationFilter>().ToArray();
        var options = new DashboardOptions
        {
            Authorization = application.Environment.IsDevelopment() ? [] : dashboardAuthorizationFilters
        };
        application.UseHangfireDashboard("/hangfire", options);
    }

    private string GenerateRedisConnectionString()
    {
        var name = Configuration["HANGFIRE_REDIS_NAME"] ?? "SaveApis";
        var host = Configuration["HANGFIRE_REDIS_HOST"] ?? "localhost";
        var port = Configuration["HANGFIRE_REDIS_PORT"] ?? "6379";
        var database = Configuration["HANGFIRE_REDIS_DATABASE"] ?? "0";
        var username = Configuration["HANGFIRE_REDIS_USERNAME"] ?? string.Empty;
        var password = Configuration["HANGFIRE_REDIS_PASSWORD"] ?? string.Empty;
        var ssl = Configuration["HANGFIRE_REDIS_SSL"] ?? "false";

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